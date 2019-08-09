using AutoMapper;
using ConfigMgmt;
using UnitTestCommon;

namespace ConfigMgmtUnitTest
{
    public class TheAppServiceTestBase : AppServiceTestBase
    {
        protected static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            return config.CreateMapper();
        }
    }
}
