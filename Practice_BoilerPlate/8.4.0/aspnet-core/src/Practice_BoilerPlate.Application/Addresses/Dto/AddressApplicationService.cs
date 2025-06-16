using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Students;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Addresses.Dto
{
    public class AddressApplicationService : ApplicationService, IAddressApplicationService
    {
        private readonly IRepository<Address> _repositoryaddress;
        public AddressApplicationService(IRepository<Address> repositoryaddress_)
        {
            _repositoryaddress = repositoryaddress_;
        }

        public async System.Threading.Tasks.Task CreateAsync(CreateAddressDto input)
        {
            try
            {
                var address = new Address
                {
                    TenantId = (int)AbpSession.TenantId,
                    Address1 = input.Address1,//input ka data utha ke ek  entity me daal raha ha
                    Address2 = input.Address2,
                    Country = input.Country,
                    State = input.State,
                    City = input.City,
                    PinCode = input.PinCode,


                };

                await _repositoryaddress.InsertAsync(address);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _repositoryaddress.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<GetAddressDto>> GetAll(GetAllAccountsInput input)
        {
            var query = _repositoryaddress.GetAll();
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(s =>
                s.Address1.Contains(input.Keyword) ||
                s.Address2.Contains(input.Keyword) ||
                s.Country.Contains(input.Keyword) ||
                s.State.Contains(input.Keyword)
            );
            }
            var address = await query.ToListAsync();
            var result = address.Select(address => new GetAddressDto
            {
                Id = address.Id,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Country = address.Country,
                State = address.State,
                City = address.City,
                PinCode = address.PinCode,
             }).ToList();
            var totalCount = result.Count();


            return new PagedResultDto<GetAddressDto>(
          totalCount,
          result);
        }

        public async System.Threading.Tasks.Task UpdateAsync(UpdateAddressDto input)
        {
            var address = await _repositoryaddress.GetAsync(input.Id);
            address.Id = input.Id;
            address.Address1 = input.Address1;
            address.Address2 = input.Address2;
            address.Country = input.Country;
            address.State = input.State;
            address.City = input.City;
            address.PinCode = input.PinCode;
          await _repositoryaddress.UpdateAsync(address);
        }
    }
}
