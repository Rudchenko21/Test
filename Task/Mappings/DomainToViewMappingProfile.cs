using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task.BLL.DTO;
using Task.ViewModel;

namespace Task.MappingUI // todo I guess, wrong folder name
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