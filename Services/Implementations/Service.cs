using Domain.Entities;
using Domain.IRepositories;
using Services.Abstraction.Dtos;
using Services.Abstraction.Interfaces;
using Services.Abstraction.Mappers;
using System.Linq.Expressions;

namespace Services.Implementations;

public class Service<Entity, Dto> : IService<Entity, Dto>
    where Entity : Base
    where Dto : BaseDto
{
    private readonly IRepository<Entity> _repository;
    public Service(IRepository<Entity> repository)
    {
        _repository = repository;
    }

    public virtual async Task<Dto> GetAsync(int id)
    {
        var entity = await _repository.GetAsync(r => r.Id == id);
        return entity?.ToDTO<Entity, Dto>();
    }

    public virtual async Task<List<Dto>> GetListAsync()
    {
        var result = await _repository.GetListAsync();
        return result.ToListDTO<Entity, Dto>();
    }

    public async Task<List<Dto>> GetListAsync(Expression<Func<Entity, bool>>? predicate)
    {
        var list = await _repository.GetListAsync(predicate);
        return list.ToListDTO<Entity, Dto>();
    }

    public virtual async Task<bool> CreateAsync(Dto dto)
    {
        _repository.Add(dto.ToEntity<Entity, Dto>());
        return await _repository.SaveChangesAsync();
    }

    public virtual async Task<bool> UpdateAsync(Dto dto)
    {
        _repository.Update(dto.ToEntity<Entity, Dto>());
        return await _repository.SaveChangesAsync();
    }

    public virtual async Task<bool> HardDeleteAsync(int id)
    {
        var entity = await _repository.GetAsync(r => r.Id == id);
        if (entity == null) return true;

        _repository.HardDelete(entity);
        return await _repository.SaveChangesAsync();
    }

    public virtual async Task<bool> SoftDeleteAsync(int id)
    {
        var entity = await _repository.GetAsync(r => r.Id == id);
        if (entity == null) return true;

        _repository.SoftDelete(entity);
        return await _repository.SaveChangesAsync();
    }

    public virtual async Task<bool> SaveAsync()
    {
        return await _repository.SaveChangesAsync();
    }
}
