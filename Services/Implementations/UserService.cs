using Domain.Entities;
using Domain.IRepositories;
using Services.Abstraction.Dtos;
using Services.Abstraction.Interfaces;

namespace Services.Implementations;

public class UserService : Service<User, UserDto>, IUserService
{
    public UserService(IRepository<User> repository)
        : base(repository)
    {
    }
}
