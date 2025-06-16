using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Practice_BoilerPlate.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Patients
{
    public class Patient: FullAuditedEntity, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Range(0, 150)]
        public int Age { get; set; }

        [Required]
        public Gender Gender { get; set; } // Enum usage

        [Required]
        [StringLength(255)]
        public string Disease { get; set; }

        [Required]
        [StringLength(100)]
        public string Doctor { get; set; }
    }
}
