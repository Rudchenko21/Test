using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Entities;

namespace Task.Test.Mapping
{
    public class EntityToTestMappingProfile : Profile // todo unused class. Remove it
    {
        public override string ProfileName
        {
            get { return "EntityToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Genre, GenreDTO>().ReverseMap();
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>().ReverseMap();
            Mapper.CreateMap<Game, GameDTO>().ReverseMap();
            Mapper.CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
