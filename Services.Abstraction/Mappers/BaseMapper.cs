using AutoMapper;
using Domain.Entities;
using Services.Abstraction.Dtos;

namespace Services.Abstraction.Mappers;

public static partial class Extension
{
    private static IMapper? baseMapper;
    private static readonly object baseMapperMapperlock = new object();
    public static IMapper BaseMapper<T, Dto>() where T : Base where Dto : BaseDto
    {
        if (baseMapper == null)
        {
            lock (baseMapperMapperlock)
            {
                if (baseMapper == null)
                {
                    baseMapper = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<T, Dto>().ReverseMap();

                        cfg.CreateMap<User, UserDto>().ReverseMap();

                    }).CreateMapper();
                }
            }

        }

        return baseMapper;
    }

    public static DTO ToDTO<T, DTO>(this T entity) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<T, DTO>(entity);
    }
    
    public static T ToEntity<T, DTO>(this DTO dto) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<DTO, T>(dto);
    }

    public static T ToEntity<T, DTO>(this DTO dto, T entity) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map(dto, entity);
    }

    public static IEnumerable<DTO> ToEnumerableDTO<T, DTO>(this IEnumerable<T> entityList) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<IEnumerable<T>, IEnumerable<DTO>>(entityList);
    }

    public static IEnumerable<T> ToEnumerableEntity<T, DTO>(this IEnumerable<DTO> dtoList) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<IEnumerable<DTO>, IEnumerable<T>>(dtoList);
    }

    public static List<DTO> ToListDTO<T, DTO>(this List<T> entityList) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<List<T>, List<DTO>>(entityList);
    }

    public static List<T> ToListEntity<T, DTO>(this List<DTO> dtoList) where T : Base where DTO : BaseDto
    {
        return BaseMapper<T, DTO>().Map<List<DTO>, List<T>>(dtoList);
    }
}
