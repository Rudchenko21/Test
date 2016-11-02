﻿using AutoMapper;
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
            Mapper.CreateMap<GameDTO, GameViewModel>().ReverseMap(); // todo remove commented code
                //.ForMember(c => c.Genre, m => m.MapFrom(s => (s.Genre.Select(s1 => new GenreDTO { Name = s1.Name, ParentId = s1.ParentId, GenreId = s1.GenreId }).ToList())))
                //.ForMember(c => c.PlatformType, m => m.MapFrom(s => (s.PlatformType.Select(s1 => new PlatformTypeDTO { Key = s1.Key, Name = s1.Name }).ToList())));

            Mapper.CreateMap<GameViewModel, GameDTO>();
            Mapper.CreateMap<GenreDTO, GenreViewModel>().ReverseMap();// todo this class named DomainToViewMappingProfile, but you use Reverse here. It's not correct. Or rename class, or create another class with all reverted mappings
            Mapper.CreateMap<PlatformTypeDTO, PlatformTypeViewModel>().ReverseMap() ;
            Mapper.CreateMap<CommentDTO, CommentViewModel>().ReverseMap();

        }
    }
}