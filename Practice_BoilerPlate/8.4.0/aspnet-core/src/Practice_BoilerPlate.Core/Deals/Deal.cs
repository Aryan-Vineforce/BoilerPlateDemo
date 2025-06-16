using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Practice_BoilerPlate.Task;
namespace Practice_BoilerPlate.Deals
{
    public class Deal: FullAuditedEntity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        public string DealName { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Task_Item> Taskitem { get; set; } = new List<Task_Item>();

    }
}
