using Domain.Entities;
using Services.Abstraction.Dtos;
using Services.Abstraction.Models;

namespace Services.Abstraction.Interfaces;

public interface IAuthService : IService<User, UserDto>
{
    Task<AuthModel> RegisterAsync(UserDto userDto);
}
