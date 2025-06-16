using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Admissions.Dto
{
    public class CreateUpdateAdmissionDto:EntityDto<int?>
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int BedId { get; set; }

        [Required]
        public DateTime AdmitDate { get; set; }

        public DateTime? DischargeDate { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
