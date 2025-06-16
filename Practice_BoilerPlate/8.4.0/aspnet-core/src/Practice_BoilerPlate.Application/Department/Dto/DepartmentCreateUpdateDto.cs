using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Department.Dto
{
    public class DepartmentCreateUpdateDto : Entity<int?>
    {

        //public int TenantId { get; set; }
        //public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }
    }
}
