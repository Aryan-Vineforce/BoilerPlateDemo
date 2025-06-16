using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.FileUploads.Dto
{
    public interface IFileUploadApplicationService :IApplicationService
    {
        Task<FileUploadDto> UploadFileAsync(FileUploadDto input);
        Task<PagedResultDto<FileGetDto>> GetAll(GetAllAccountsInput input);
        System.Threading.Tasks.Task UpdateAsync(FileUpdateDto input);
        System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input);
    }
}
