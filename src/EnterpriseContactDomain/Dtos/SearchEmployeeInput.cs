using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    /// <summary>
    /// 搜索选项
    /// </summary>
    [Serializable]
    public class SearchEmployeeInput
    {
        public SearchEmployeeInput()
        {
            this.PageIndex = 1;
            this.PageSize = 20;
        }

        public Guid EmpId { get; set; }

        public string Name { get; set; }

        public string Number { get; set; }

        public string IdCardNo { get; set; }

        public Guid? UserId { get; set; }

        public string UserName { get; set; }

        /// <summary>
        /// 用户所属的部门,包括兼职
        /// </summary>
        public Guid DepartmentId { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    public enum SearchPattern
    {
        IgnoreCase = 1,

        NotIgnoreCase = 2,
    }
}
