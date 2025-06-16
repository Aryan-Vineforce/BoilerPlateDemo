using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Practice_BoilerPlate.Employees;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Practice_BoilerPlate.Departments
{
    public class Departmentt : FullAuditedEntity<int>, IMustHaveTenant
    {
        public int TenantId { get; set; }
        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Department code is required.")]
        [StringLength(10, ErrorMessage = "Code can't be longer than 10 characters.")]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = "Description can't be longer than 500 characters.")]
        public string Description { get; set; }

        // Navigation property
        public virtual ICollection<Employeee> Employees { get; set; }
    }
}
