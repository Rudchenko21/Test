using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Task.BLL.DTO;
using Task.ViewModel;

namespace Task.MappingUI
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