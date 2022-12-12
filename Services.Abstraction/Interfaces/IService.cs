using Domain.Entities;
using Services.Abstraction.Dtos;
using System.Linq.Expressions;

namespace Services.Abstraction.Interfaces;

public interface IService<Entity, Dto>
    where Entity : Base
    where Dto : BaseDto
{
    Task<Dto> GetAsync(int id);
    Task<List<Dto>> GetListAsync();
    Task<List<Dto>> GetListAsync(Expression<Func<Entity, bool>>? predicate);

    Task<bool> CreateAsync(Dto dto);
    Task<bool> UpdateAsync(Dto dto);

    Task<bool> SoftDeleteAsync(int id);
    Task<bool> HardDeleteAsync(int id);
}
