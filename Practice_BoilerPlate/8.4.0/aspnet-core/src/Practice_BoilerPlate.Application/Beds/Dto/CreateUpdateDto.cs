using Abp.Application.Services.Dto;
using BoilerPlate_New.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate_New.Beds.Dto
{
    public class CreateUpdateDto: EntityDto<int>
    {
        public string BedNumber { get; set; }

        [Required]
        public BedType Type { get; set; }

        [Required]
        public BedStatus Status { get; set; }
    }
}
