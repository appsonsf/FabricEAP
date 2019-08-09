using AutoMapper;
using ConfigMgmt.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigMgmt
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppEntrance, AppEntranceDto>()
                .AfterMap((s, d) => {
                    d.ShowBadge = s.ComponentConfig?.ShowBadge == true;
                });
        }
    }
}
