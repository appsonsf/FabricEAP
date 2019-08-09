using AutoMapper;
using EnterpriseContact.ViewModels;
using System;

namespace EnterpriseContact
{
    public class VmMappingProfile : Profile
    {
        public VmMappingProfile()
        {
            CreateMap<DepartmentListOutput, DepartmentListVm>();
            CreateMap<DepartmentListOutput, DepartmentSearchListVm>();

            CreateMap<EmployeeListOutput, EmployeeListVm>().
                AfterMap((s, d) =>
                {
                    d.IsPrimary = s.PositionId == s.PrimaryPositionId;
                });
            CreateMap<EmployeeOutput, EmployeeDetailVm>();

            CreateMap<GroupListOutput, GroupListVm>();
            CreateMap<GroupOutput, GroupDetailVm>();
            CreateMap<GroupEditVm, GroupInput>();
            CreateMap<GroupMemberOutput, GroupMemberListVm>();
        }
    }
}
