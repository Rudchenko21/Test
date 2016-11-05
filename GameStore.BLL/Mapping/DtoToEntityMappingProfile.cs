using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.DAL.Entities;

namespace GameStore.BLL.Mapping
{
    public class DtoToEntityMappingProfile:Profile
    {
        public override string ProfileName
        {
            get { return "DtoToEntityMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<GenreDTO, Genre>();
            Mapper.CreateMap<PlatformTypeDTO, PlatformType>();
            Mapper.CreateMap<GameDTO,Game>();
            Mapper.CreateMap<CommentDTO, Comment>();
        }
    }
}
