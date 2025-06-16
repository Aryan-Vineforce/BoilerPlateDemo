using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.BedStatus.DTo;
using Practice_BoilerPlate.Enrollments.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.BedStatus
{
    public interface IBedStatusAppSerive:IApplicationService
    {
        Task<List<BedStatusPieChartDto>> GetAll(GetAllAccountsInput getAllAccountInput);
    }
}
