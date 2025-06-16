using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Task.Dto
{
    public class CreateUpdateTaskDto: Entity<int?>
    {
public int TaskNo { get; set; } 
        public string Title { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; }
        //public int DealId { get; set; }
        //public string DealName { get; set; }
        //public DateTime Date { get; set; }
    }
}
