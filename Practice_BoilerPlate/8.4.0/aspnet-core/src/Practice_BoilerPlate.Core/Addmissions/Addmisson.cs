using BoilerPlate_New.Beds;
using Practice_BoilerPlate.Patients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Practice_BoilerPlate.Addmissions
{
    public class Addmisson : FullAuditedEntity, IMustHaveTenant
    {

        public int TenantId { get; set; }
        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }

        [Required]
        [ForeignKey("Bed")]
        public int BedId { get; set; }

        [Required]
        public DateTime AdmitDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        public string? Notes { get; set; }

        // Navigation properties
        public  Patient Patient { get; set; }

        public Bed Bed { get; set; }
    }
}
