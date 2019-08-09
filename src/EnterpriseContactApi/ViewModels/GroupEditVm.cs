using System;
using System.Collections.Generic;
using System.Text;

namespace EnterpriseContact.ViewModels
{
    public class GroupEditVm
    {
        /// <summary>
        /// 在Update的时候如果为null，即跳过此属性的更新
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 在Update的时候如果为null，即跳过此属性的更新
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 要添加成员的员工Id的集合，在Create的时候设置这个集合
        /// </summary>
        public HashSet<Guid> AddingMemberIds { get; set; }

        /// <summary>
        /// 要添加成员的员工名称集合，和 <see cref="AddingMemberIds" /> 顺序保持一致
        /// </summary>
        public List<string> AddingMemberNames { get; set; }

        /// <summary>
        /// 要删除成员的员工Id的集合
        /// </summary>
        public HashSet<Guid> RemovingMemberIds { get; set; }

        /// <summary>
        ///  要删除成员的员工名称集合，和 <see cref="RemovingMemberIds"/> 顺序保持一致
        /// </summary>
        public List<string> RemovingMemberNames { get; set; }
    }
}
