using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContact.Entities
{
    /// <summary>
    /// 联系人组
    /// </summary>
    public class Group
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { get; set; }

        //[Index]
        [Required]
        public GroupType Type { get; set; }

        /// <summary>
        /// 联系人组的备注信息
        /// </summary>
        public string Remark { get; set; }

        public int? IconId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTimeOffset Updated { get; set; }

        /// <summary>
        /// 创建者UserId，如果是白名单组，那么应该是管理员的UserId
        /// </summary>
        public Guid CreatedBy { get; set; }

        //TODO public bool IsDeleted { get; set; }

        public virtual List<GroupMember> Members { get; set; }
    }
}
