using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Practice_BoilerPlate.Courses;
namespace Practice_BoilerPlate.Subjects
{
    public class Subject : FullAuditedEntity, IMustHaveTenant

    {
        public int TenantId { get; set; }
        public string Name { get; set; }

        public string Code { get; set; }

        public int Credits { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
