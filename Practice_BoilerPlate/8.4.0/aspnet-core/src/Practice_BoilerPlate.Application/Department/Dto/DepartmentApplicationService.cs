using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Department.Dto;
using Practice_BoilerPlate.Departments;
using Practice_BoilerPlate.Students.Dto;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;


namespace Practice_BoilerPlate.Department
{
    public class DepartmentApplicationService:ApplicationService,IDepartmentAppService
    {
        private readonly IRepository<Departmentt> _departmentrepository;
        public DepartmentApplicationService(IRepository<Departmentt> departmentrepository )
        {
            _departmentrepository = departmentrepository;

        }

        public async System.Threading.Tasks.Task CreateAsync(DepartmentCreateUpdateDto input)
        {
            var department = new Departmentt
            {
                TenantId = (int)AbpSession.TenantId,
                Name = input.Name,
                Code = input.Code,
                Description = input.Description
            };

            await _departmentrepository.InsertAsync(department);
            // Make sure Id is generated

            
        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _departmentrepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetDepartmentDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _departmentrepository.GetAll();

            // Filtering
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(d =>
                    d.Name.Contains(input.Keyword) ||
                    d.Code.Contains(input.Keyword) ||
                    d.Description.Contains(input.Keyword));
            }

            // Sorting - default to Name if no Sorting is specified
            query = !string.IsNullOrWhiteSpace(input.Sorting)
                ? query.OrderBy(input.Sorting)
                : query.OrderBy(d => d.Name);

            // Paging
            var pagedDepartments = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            // Mapping
            var result = pagedDepartments.Select(department => new GetDepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code,
                Description = department.Description
            }).ToList();

            var totalCount = await query.CountAsync();

            return new PagedResultDto<GetDepartmentDto>(totalCount, result);
        }


        public async System.Threading.Tasks.Task UpdateAsync(UpdateDepartmentDto input)
        {
            var department = await _departmentrepository.GetAsync(input.Id);
            department.Name = input.Name;
            department.Code = input.Code;
            department.Description = input.Description;

            await _departmentrepository.UpdateAsync(department);
        }


    }
}
