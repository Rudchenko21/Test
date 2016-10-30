using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Task.BLL.Mapping;
using Task.Test.Mapping;

namespace Task.MappingUI
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<EntityToDomainMappingProfile>();
                x.AddProfile<DomainToViewMappingProfile>();
            });
        }
    }
}