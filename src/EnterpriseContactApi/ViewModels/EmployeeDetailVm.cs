using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class EmployeeDetailVm
    {
        /// <summary>
        /// 员工id
        /// </summary>
        public Guid Id { get; set; }

        public string Number { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string Name { get; set; }

        public Guid? UserId { get; set; }
      
        /// <summary>
        /// 员工的的账号名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 部门id
        /// </summary>
        //public Guid DepartmentId { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public List<string> DepartmentNames { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 父级部门Id
        /// </summary>
        //public Guid ParentDepartmentId { get; set; }

        /// <summary>
        /// 父级部门名称
        /// </summary>
        //public string ParentDepartmentName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        //public string Mail { get; set; }

        /// <summary>
        /// 是否在同一个白名单组当中，可以发送P2P消息
        /// </summary>
        public bool SameWhiteListGroup { get; set; }

        /// <summary>
        /// 此员工是否已经收藏
        /// </summary>
        public bool IsFavorited { get; set; }

        //public List<DepartmentListVm> DepartmentHierarchy { get; set; }

        public List<ParttimeJobListVm> ParttimeJobs { get; set; }
    }
}
