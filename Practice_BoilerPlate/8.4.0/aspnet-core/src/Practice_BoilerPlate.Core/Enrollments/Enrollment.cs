using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Practice_BoilerPlate.Courses;
using Practice_BoilerPlate.Students;
using System;


namespace Practice_BoilerPlate.Enrollments
{
    public class Enrollment:FullAuditedEntity, IMustHaveTenant
    {


        public int TenantId { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; }


        public int CourseId { get; set; }

        public Course Course { get; set; }

  

        
    }
}
