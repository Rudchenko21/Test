using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task.BLL.Mapping;

namespace Task.MappingUI
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<EntityToDtoMappingProfile>();
                x.AddProfile<DomainToViewMappingProfile>();
                x.AddProfile<ViewToDomainMappingProfile>();
                x.AddProfile<DtoToEntityMappingProfile>();
            });
        }
    }
}