using Abp.Domain.Entities;
using Practice_BoilerPlate.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Patients.Dto
{
        public class CreateUpdatePatientDto:Entity<int>
        {
            [Required]
            [MaxLength(100)]
            public string Name { get; set; }

            [Range(0, 100)]
            public int Age { get; set; }

            [Required]
            public Gender Gender { get; set; }

            [MaxLength(256)]
            public string Disease { get; set; }

            [MaxLength(100)]
            public string Doctor { get; set; }
        }
    }
