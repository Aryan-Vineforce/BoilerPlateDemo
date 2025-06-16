using Abp.Domain.Entities;
using Practice_BoilerPlate.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Patients.Dto
{
    public class GetPatientDto:Entity<int>
    {
        
        public string Name { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string Disease { get; set; }
        public string Doctor { get; set; }
    }
}
