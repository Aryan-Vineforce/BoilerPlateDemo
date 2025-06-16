using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;

using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Patients.Dto;
using Practice_BoilerPlate.Students.Dto;

using System.Linq;

using System.Threading.Tasks;

namespace Practice_BoilerPlate.Patients
{
    public class PatientAppService : ApplicationService, IPatientAppService
    {
        private readonly IRepository<Patient> _patientRepo;
        public PatientAppService(IRepository<Patient> patientRepo)
        {
            _patientRepo = patientRepo;
        }

        public async System.Threading.Tasks.Task CreateAsync(CreateUpdatePatientDto input)
        {
             var department = new Patient
            {
                TenantId = (int)AbpSession.TenantId,
             
                 Name = input.Name,
                 Age = input.Age,
                 Gender = input.Gender,
                 Disease = input.Disease,
                 Doctor = input.Doctor
             };

            await _patientRepo.InsertAsync(department);
           
        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _patientRepo.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetPatientDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _patientRepo.GetAll()
       .Where(x => x.TenantId == AbpSession.TenantId);

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(input.Keyword) ||
                    p.Gender.ToString().Contains(input.Keyword) ||
                    p.Disease.Contains(input.Keyword) ||
                    p.Doctor.Contains(input.Keyword));
            }

            var totalCount = await query.CountAsync();

            var patients = await query
                .OrderByDescending(p => p.CreationTime)
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var result = patients.Select(p => new GetPatientDto
            {
                Id = p.Id,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                Disease = p.Disease,
                Doctor = p.Doctor
            }).ToList();

            return new PagedResultDto<GetPatientDto>(totalCount, result);
        }

        

        public async System.Threading.Tasks.Task UpdateAsync(CreateUpdatePatientDto input)
        {
            var patient = await _patientRepo.GetAsync((int)input.Id);

            patient.Name = input.Name;
            patient.Age = input.Age;
            patient.Gender = input.Gender;
            patient.Disease = input.Disease;
            patient.Doctor = input.Doctor;

            await _patientRepo.UpdateAsync(patient);
        }
    }
}
