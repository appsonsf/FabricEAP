using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Security.Claims;

namespace UnitTestCommon
{
    public abstract class ControllerTestBase
    {
        protected static IMemoryCache CreateMemoryCache()
        {
            return new MemoryCache(new MemoryCacheOptions());
        }

        protected static readonly Guid User_Id = new Guid("6c8cb9c0dbc4479ca6486e97dc9b8b75");
        protected static readonly Guid User_EmployeeMdmId = new Guid("85a270bd81144a12b380a22ffe9527d4");
        protected const string User_Name = "user1";
        protected const string User_Mobile = "13812345678";
        protected const string User_EmployeeNumber = "001";
        protected const string User_EmployeeName = "张三";
        protected const string User_IdCardNo = "123";
        protected static readonly string[] User_Roles = new string[] { "BI" };

        protected static ControllerContext CreateMockContext()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(ClaimTypes.NameIdentifier, User_Id.ToString()),
                 new Claim("preferred_username", User_Name),
                 new Claim("name", User_EmployeeName),
                 new Claim("employee_number", User_EmployeeNumber),
                 new Claim("employee_mdmid", User_EmployeeMdmId.ToString()),
                 new Claim("idcard_number", User_IdCardNo),
                 new Claim("phone_number", User_Mobile),
                 new Claim(ClaimTypes.Role, string.Join(',', User_Roles)),
            }, "jwt"));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}
