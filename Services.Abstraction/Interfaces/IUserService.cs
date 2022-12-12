using Domain.Entities;
using Services.Abstraction.Dtos;

namespace Services.Abstraction.Interfaces;

public interface IUserService : IService<User, UserDto>
{
}
