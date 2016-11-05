using AutoMapper;
using GameStore.BLL.Mapping;

namespace GameStore.WEB.Mapping
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<EntityToDtoMappingProfile>();
                x.AddProfile<DomainToViewMappingProfile>();
                x.AddProfile<ViewToDomainMappingProfile>();
                x.AddProfile<DtoToEntityMappingProfile>();
            });
        }
    }
}