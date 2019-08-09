using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact
{
    [Serializable]
    public class GroupOutput
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 联系人组的名称
        /// </summary>
        public string Name { get; set; }

        public GroupType Type { get; set; }

        /// <summary>
        /// 联系人组的备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset Updated { get; set; }

        public Guid CreatedBy { get; set; }

        /// <summary>
        /// 关联的成员的Id
        /// </summary>
        public List<GroupMemberOutput> Members { get; set; }
    }
}
