using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.BLL.DTO;
using Task.DAL.Entities;

namespace Task.BLL.Mapping
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
