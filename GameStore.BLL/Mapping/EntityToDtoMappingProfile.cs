using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Mapping
{
    public class EntityToDtoMappingProfile : Profile // 
    {
        public override string ProfileName
        {
            get { return "EntityToDtoMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Genre, GenreDTO>();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>();
            Mapper.CreateMap<Game, GameDTO>();
            Mapper.CreateMap<Comment, CommentDTO>()
                .ForMember(m=>m.GameKey,c=>c.MapFrom(s=>s.Game.Key))
                .ForMember(m=>m.GameId,c=>c.MapFrom(s=>s.Game.Id));
        }
    }
}
