using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using BoilerPlate_New.Beds.Dto;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate_New.Beds
{
    public class BedApplicationService:ApplicationService,IBedApplicationService
    {
        private readonly IRepository<Bed> _bedrepositroty;
        public BedApplicationService(IRepository<Bed> bedrepository)
        {
            _bedrepositroty = bedrepository;
        }

        public async Task CreateAsync(CreateUpdateDto input)
        {
            //try
            //{
            //    var tenantId = AbpSession.TenantId ?? throw new UserFriendlyException("TenantId is null.");

            //    // Step 1: Get all used bed numbers (even deleted ones)
            //    var usedNumbers = await _bedrepositroty.GetAll()
            //        .IgnoreQueryFilters() // include soft-deleted beds
            //        .Where(b => b.TenantId == tenantId)
            //        .Select(b => b.BedNumber)
            //        .ToListAsync();

            //    // Safely filter valid integers
            //    var usedNumberSet = usedNumbers
            //        .Where(b => !string.IsNullOrEmpty(b) && int.TryParse(b, out _))
            //        .Select(int.Parse)
            //        .ToHashSet();

            //    // Step 2: Find the lowest available number
            //    int newNumber = 1;
            //    while (usedNumberSet.Contains(newNumber))
            //    {
            //        newNumber++;
            //    }

            //    string newBedNumber = newNumber.ToString();

            //    // Step 3: Create bed
            //    var bed = new Bed
            //    {
            //        TenantId = (int)tenantId,
            //        BedNumber = newBedNumber,
            //        Type = input.Type,
            //        Status = input.Status
            //    };

            //    await _bedrepositroty.InsertAsync(bed);
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException("An error occurred while creating the bed.", ex);
            //}
            var tenantId = AbpSession.TenantId ?? throw new UserFriendlyException("TenantId is null.");

            // Optional: Validate unique BedNumber per tenant
            var existingBed = await _bedrepositroty.FirstOrDefaultAsync(b =>
                b.TenantId == tenantId && b.BedNumber == input.BedNumber);

            if (existingBed != null)
            {
                throw new UserFriendlyException("BedNumber already exists.");
            }

            var bed = new Bed
            {
                TenantId = (int)tenantId,
                BedNumber = input.BedNumber,
                Type = input.Type,
                Status = input.Status
            };

            await _bedrepositroty.InsertAsync(bed);
        }

        public async Task DeleteAsync(EntityDto<int> input)
        {
            await _bedrepositroty.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetBedDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _bedrepositroty.GetAll();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(d =>
                d.BedNumber.Contains(input.Keyword) );
            }
            query = !string.IsNullOrWhiteSpace(input.Sorting)
                     ? query.OrderBy(input.Sorting)
                     : query.OrderBy(d => d.Status).OrderBy(d => d.BedNumber).OrderBy(b => b.Type);
                     
            var totalCount = await query.CountAsync();

           

            var beds = await query
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToListAsync();

            var result = beds.Select(b => new GetBedDto
            {
                Id = b.Id,
                BedNumber = b.BedNumber,
                Type = b.Type,
                Status = b.Status
            }).ToList();

            return new PagedResultDto<GetBedDto>(totalCount, result);
        }

        public async Task UpdateAsync(CreateUpdateDto input)
        {
            var bed = await _bedrepositroty.GetAsync(input.Id);

            bed.BedNumber = input.BedNumber;
            bed.Type = input.Type;
            bed.Status = input.Status;

            await _bedrepositroty.UpdateAsync(bed);
        }
    }
}
