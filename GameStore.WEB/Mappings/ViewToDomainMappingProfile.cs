using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.WEB.ViewModel;

namespace GameStore.WEB.Mapping
{
    public class ViewToDomainMappingProfile:Profile
    {
        public override string ProfileName
        {
            get { return "ViewToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<GameViewModel,GameDTO>();
            Mapper.CreateMap<GenreViewModel,GenreDTO>();
            Mapper.CreateMap<PlatformTypeViewModel,PlatformTypeDTO>();
            Mapper.CreateMap<CommentViewModel,CommentDTO>();
        }
    }
}