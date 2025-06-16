using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoilerPlate_New.Enums;
using Microsoft.EntityFrameworkCore;

namespace BoilerPlate_New.Beds
{
    [Index(nameof(BedNumber), IsUnique = true)]
    public class Bed: FullAuditedEntity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }

        [Required]
        [MaxLength(20)]
        public string BedNumber { get; set; }


        public BedType Type { get; set; }

        [Required]
        public BedStatus Status { get; set; }
    }
}
