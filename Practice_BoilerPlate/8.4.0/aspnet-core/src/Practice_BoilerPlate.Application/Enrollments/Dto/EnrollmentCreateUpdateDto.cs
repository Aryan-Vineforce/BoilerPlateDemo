using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Courses;
using Practice_BoilerPlate.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Enrollments.Dto
{
    public class EnrollmentCreateUpdateDto:EntityDto<int>
    {
 
        public int StudentId { get; set; }

        public List<int> CourseIds { get; set; }
        public DateTime EnrollmentDate { get; set; }
    }
}
