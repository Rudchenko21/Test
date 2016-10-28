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
    public class EntityToDomainMappingProfile : Profile
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
            //.ForMember(c => c.Genre, m => m.MapFrom(s => (s.Genre.Select(s1 => new Genre {Name= s1.Name,ParentId=s1.ParentId,GenreId = s1.GenreId }).ToList())))
            //.ForMember(c => c.PlatformType, m => m.MapFrom(s => (s.PlatformType.Select(s1 => new PlatformType { Key=s1.Key,Name=s1.Name }).ToList())));
            Mapper.CreateMap<GameDTO, Game>();
            //    .ForMember(c => c.Genre, m => m.MapFrom(s => (s.Genre.Select(s1 => new GenreDTO { Name = s1.Name, ParentId = s1.ParentId, GenreId = s1.GenreId }).ToList())))
            //    .ForMember(c => c.PlatformType, m => m.MapFrom(s => (s.PlatformType.Select(s1 => new PlatformTypeDTO { Key = s1.Key, Name = s1.Name }).ToList())));
            Mapper.CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
