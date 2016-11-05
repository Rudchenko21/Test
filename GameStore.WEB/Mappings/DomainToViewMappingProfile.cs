using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.WEB.ViewModel;

namespace GameStore.WEB.Mapping
{
    public class DomainToViewMappingProfile:Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewMappings"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<GameDTO, GameViewModel>();
            Mapper.CreateMap<GenreDTO, GenreViewModel>();
            Mapper.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>();
            Mapper.CreateMap<CommentDTO, CommentViewModel>();
        }
    }
}