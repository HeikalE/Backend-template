using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Dtos;
using Services.Abstraction.Interfaces;
using Services.Abstraction.Mappers;
using Services.Abstraction.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Implementations;

public class AuthService : Service<User, UserDto>, IAuthService
{
    private readonly JwtModel _jwt;
    private readonly IRepository<User> _repository;
    public AuthService(IRepository<User> repository, IOptions<JwtModel> jwt)
        : base(repository)
    {
        _repository = repository;
        _jwt = jwt.Value;
    }

    public async Task<AuthModel> RegisterAsync(UserDto userDto)
    {
        var user = await _repository.GetAsync(r => r.Email == userDto.Email || r.PhoneNumber == userDto.PhoneNumber);
        if (user is not null && user.Email == user.Email)
        {
            return new AuthModel { Message = "Email is already registered!" };
        }

        if (user is not null && user.PhoneNumber == user.PhoneNumber)
        {
            return new AuthModel { Message = "Phone Numer is already registered!" };
        }

        AuthModel authModel = new AuthModel { Message = "" };

        var newUser = userDto.ToEntity<User, UserDto>();
        _repository.Add(newUser);

        bool isAdded = await _repository.SaveChangesAsync();
        if (isAdded)
        {
            authModel.Message = "User is added successfully";
            authModel.IsAuthenticated = true;
            authModel = GeneratedToken(authModel, newUser);
        }
        else
        {
            authModel.Message = "Some thing error happens";
        }

        return authModel;
    }

    private AuthModel GeneratedToken(AuthModel authModel, User user)
    {
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("id" , user.Id.ToString()),
            new Claim("name" , user.Name ?? ""),
            new Claim("email" , user.Email ?? ""),
            new Claim("phone" , user.PhoneNumber ?? ""),
            new Claim("role" , user.Role.ToString())
        };

        var expireOn = DateTime.Now.AddDays(_jwt.DurationInDays);
        var token = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            expires: expireOn,
            claims: claims,
            signingCredentials: signingCredentials
            );

        authModel.ExpiresOn = expireOn;
        authModel.Token = new JwtSecurityTokenHandler().WriteToken(token);

        return authModel;
    }
}
