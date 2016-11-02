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
    public class EntityToDomainMappingProfile : Profile // todo maybe EntityToDtoMappingProfile ?
    {
        public override string ProfileName
        {
            get { return "EntityToDomainMappings"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<Genre, GenreDTO>().ReverseMap(); // todo such comment as at DomainToViewMappingProfile
            Mapper.CreateMap<PlatformType, PlatformTypeDTO>().ReverseMap();
            Mapper.CreateMap<Game, GameDTO>().ReverseMap();
            Mapper.CreateMap<Comment, CommentDTO>()
                .ForMember(m=>m.GameKey,c=>c.MapFrom(s=>s.Game.Key))
                .ForMember(m=>m.GameId,c=>c.MapFrom(s=>s.Game.Id));
            Mapper.CreateMap<CommentDTO, Comment>();
        }
    }
}
