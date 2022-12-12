using Domain.IRepositories;
using Infrastructure.Context;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Interfaces;
using Services.Abstraction.Models;
using Services.Implementations;
using System.Text;

namespace WebAPI.Extensions;

public static class SetupExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
            });
        });
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration Configuration) 
    {
        services.Configure<JwtModel>(Configuration.GetSection("JWT"));

        services.AddAuthentication(options => 
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt => 
        {
            opt.RequireHttpsMetadata = false;
            opt.SaveToken = false;
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = Configuration["JWT:Issuer"],
                ValidAudience = Configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
            };
        });
    }

    public static void RegisterRepositories(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddDbContext<ADbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("default")));
        //services.AddDbContext<ADbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IUserService, UserService>();
    }

}
