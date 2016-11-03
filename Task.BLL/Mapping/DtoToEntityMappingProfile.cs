using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Task.BLL.DTO;
using Task.DAL.Entities;

namespace Task.BLL.Mapping
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
