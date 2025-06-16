using Abp.Domain.Entities;
using Practice_BoilerPlate.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Deals.Dto
{
    public class DealDto:Entity<int>
    {
        public string DealName { get; set; }
        public DateTime Date { get; set; }
        public List<TaskDto> Tasks { get; set; }
        public TaskDto DeletedTask { get; internal set; }
    }
}
