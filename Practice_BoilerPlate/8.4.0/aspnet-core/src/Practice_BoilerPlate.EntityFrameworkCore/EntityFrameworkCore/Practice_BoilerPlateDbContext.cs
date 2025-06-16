using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using Practice_BoilerPlate.Authorization.Roles;
using Practice_BoilerPlate.Authorization.Users;
using Practice_BoilerPlate.MultiTenancy;
using Practice_BoilerPlate.Students;
using Practice_BoilerPlate.Departments;
using Practice_BoilerPlate.Employees;
using Practice_BoilerPlate.Courses;
using Practice_BoilerPlate.Teachers;
using Practice_BoilerPlate.Addresses;
using Practice_BoilerPlate.Subjects;
using Practice_BoilerPlate.TeacherSubjects;
using Practice_BoilerPlate.Enrollments;
using Practice_BoilerPlate.FileUploads;
using BoilerPlate_New.Beds;
using Practice_BoilerPlate.Patients;
using Practice_BoilerPlate.Addmissions;
using Practice_BoilerPlate.Deals;
using Practice_BoilerPlate.Task;

namespace Practice_BoilerPlate.EntityFrameworkCore
{
    public class Practice_BoilerPlateDbContext : AbpZeroDbContext<Tenant, Role, User, Practice_BoilerPlateDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public Practice_BoilerPlateDbContext(DbContextOptions<Practice_BoilerPlateDbContext> options)
            : base(options)
        {
        }
        public DbSet<Student> Student { get; set; }
        public DbSet<Departmentt> Departments { get; set; }
        public DbSet<Employeee> Employees { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet <Address>Addresses { get; set; }
        public DbSet <Subject>Subjects { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<FileUpload> FileUploads { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Addmisson> Addmissions { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Task_Item> TasksItems { get; set; }



    }
}
