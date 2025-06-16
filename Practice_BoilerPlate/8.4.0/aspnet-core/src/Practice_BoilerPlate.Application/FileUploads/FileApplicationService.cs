using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Practice_BoilerPlate.FileUploads.Dto;
using Practice_BoilerPlate.Students.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.FileUploads
{
    public class FileApplicationService : ApplicationService, IFileUploadApplicationService
    {
        private readonly IRepository<FileUpload> _filerepository;
        public FileApplicationService(IRepository<FileUpload> filerepository)
        {
            _filerepository = filerepository;
        }

        public async System.Threading.Tasks.Task DeleteAsync(EntityDto<int> input)
        {
            await _filerepository.DeleteAsync(input.Id);
        }

        public async Task<PagedResultDto<FileGetDto>> GetAll(GetAllAccountsInput input)
        {
            try
            {
                var query = _filerepository.GetAll();

                if (!string.IsNullOrWhiteSpace(input.Keyword))
                {
                    query = query.Where(x => x.FileName.Contains(input.Keyword));
                }

                var totalCount = await query.CountAsync();

                var fileList = await query
                    .OrderByDescending(x => x.CreationTime)
                    .Skip(input.SkipCount)
                    .Take(input.MaxResultCount)
                    .ToListAsync();

                var result = fileList.Select(file => new FileGetDto
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    FilePath = file.FilePath
                }).ToList();

                return new PagedResultDto<FileGetDto>(totalCount, result);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while fetching uploaded files", ex);
                throw;
            }
        }

        public async System.Threading.Tasks.Task UpdateAsync(FileUpdateDto input)
        {
            try
            {
                // Retrieve the file upload record by ID
                var fileUpload = await _filerepository.GetAsync(input.Id);

                if (fileUpload == null)
                {
                    throw new Exception("File upload record not found");
                }

                // Update file details
                fileUpload.FileName = input.FileName;
                fileUpload.FilePath = input.FilePath; // Update file path if necessary

                // Save changes to the database
                await _filerepository.UpdateAsync(fileUpload);

                // Return updated FileUploadDto
                var fileUploadDto = new FileUploadDto
                {
                    Id = fileUpload.Id,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath
                };


            }

            catch (Exception ex)
            {
                Logger.Error("Error while updating file", ex);
                throw;  // Rethrow the exception to maintain ABP's error handling
            }
        }
        

        public async Task<FileUploadDto> UploadFileAsync(FileUploadDto input)
        {
            try
            {
                // Define file upload directory
                var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");

                // Create directory if it does not exist
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                // Generate file path
                var filePath = Path.Combine(fileDirectory, input.FileName);

                // Convert Base64 string to byte array and write to file
                var fileBytes = Convert.FromBase64String(input.FilePath);
                await File.WriteAllBytesAsync(filePath, fileBytes);

                // Create a FileUpload entity
                var fileUpload = new FileUpload
                {
                    FileName = input.FileName,
                    FilePath = "/upload/" + input.FileName  // Save relative path
                };

                // Save entity to database
                await _filerepository.InsertAsync(fileUpload);

                // Manually map entity to DTO (without AutoMapper)
                var resultDto = new FileUploadDto
                {
                    Id = fileUpload.Id,
                    FileName = fileUpload.FileName,
                    FilePath = fileUpload.FilePath
                };  

                return resultDto;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while uploading file", ex);
                throw;
            }
        }
    }
    }

