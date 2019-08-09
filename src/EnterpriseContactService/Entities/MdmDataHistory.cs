using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseContactService.Entities
{
    /// <summary>
    /// mdm 数据历史记录
    /// </summary>
    public class MdmDataHistory
    {
        public Guid Id { get; set; }


        [Required]
        [StringLength(64)]
        public string HistoryVersion { get; set; }

        public DateTimeOffset SyncTime { get; set; }

        [StringLength(256)]
        public string Description { get; set; }
    }
}
