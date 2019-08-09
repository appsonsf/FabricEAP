using AutoMapper;
using EnterpriseContact;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;
using UnitTestCommon;

namespace EnterpriseContactUnitTest
{
    public class TheControllerTestBase : ControllerTestBase
    {
        protected static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VmMappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}
