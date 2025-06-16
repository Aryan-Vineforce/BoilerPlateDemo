using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Practice_BoilerPlate.Deals.Dto;
using Practice_BoilerPlate.Students.Dto;
using Practice_BoilerPlate.Task;
using Practice_BoilerPlate.Task.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Deals
{
    public class DealWithTasksAppService : ApplicationService, IDealWithTasksAppService
    {
        private readonly IRepository<Deal> _dealrepo;
        private readonly IRepository<Task_Item> _taskitemrepo;
        public DealWithTasksAppService(IRepository<Deal> dealrepo, IRepository<Task_Item> taskrepo)
        {

            _dealrepo = dealrepo;
            _taskitemrepo = taskrepo;
        }
        public async Task<DealDto> CreateDealWithTasksAsync(CreateUpdateDealDto input)
        {
            try
            {
                Deal deal;

                if (input.Id != null)
                {
                    throw new UserFriendlyException("Cannot create deal with existing Id.");
                }

                deal = new Deal
                {
                    DealName = input.DealName,
                    Date = input.Date
                };

                await _dealrepo.InsertAsync(deal);
                await CurrentUnitOfWork.SaveChangesAsync(); // generate DealId

                var taskDtos = new List<TaskDto>();
                int taskNo = 1;

                foreach (var taskDto in input.Tasks)
                {
                    var task = new Task_Item
                    {
                        TaskNo = taskNo++, // auto-increment
                        Title = taskDto.Title,
                        DateFrom = taskDto.DateFrom,
                        ToDate = taskDto.ToDate,
                        Description = taskDto.Description,
                        DealId = deal.Id
                    };

                    await _taskitemrepo.InsertAsync(task);

                    taskDtos.Add(new TaskDto
                    {
                        Id = task.Id,
                        TaskNo = task.TaskNo,
                        Title = task.Title,
                        DateFrom = task.DateFrom,
                        ToDate = task.ToDate,
                        Description = task.Description
                    });
                }

                return new DealDto
                {
                    Id = deal.Id,
                    DealName = deal.DealName,
                    Date = deal.Date,
                    Tasks = taskDtos
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while creating the deal: " + ex.Message);
            }
        }
        public async Task<DealDto> DeleteAsync(EntityDto<int> input)
        {
            var deal = await _dealrepo.GetAsync(input.Id);
            if (deal == null)
            {
                throw new UserFriendlyException("Deal not found.");
            }

            // Delete all associated tasks
            var tasks = await _taskitemrepo.GetAllListAsync(t => t.DealId == deal.Id);
            foreach (var task in tasks)
            {
                await _taskitemrepo.DeleteAsync(task);
            }

            // Delete the deal
            await _dealrepo.DeleteAsync(deal);

            // Return deleted deal info
            return new DealDto
            {
                Id = deal.Id,
                DealName = deal.DealName,
                Date = deal.Date,
                Tasks = tasks.Select(t => new TaskDto
                {
                    Id = t.Id,
                    TaskNo = t.TaskNo,
                    Title = t.Title,
                    DateFrom = t.DateFrom,
                    ToDate = t.ToDate,
                    Description = t.Description
                }).ToList()
            };
        }

        public async Task<TaskDto> DeleteTaskAsync(EntityDto<int> input)
        {
            var task = await _taskitemrepo.FirstOrDefaultAsync(input.Id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }

            // Save the data before deleting to return in response
            var deletedTaskDto = new TaskDto
            {
                Id = task.Id,
                TaskNo = task.TaskNo,
                Title = task.Title,
                DateFrom = task.DateFrom,
                ToDate = task.ToDate,
                Description = task.Description
            };

            await _taskitemrepo.DeleteAsync(task);

            return deletedTaskDto;
        }

        public async Task<PagedResultDto<DealDto>> GetDealWithTasksAsync(GetAllAccountsInput input)
        {
            var query = _dealrepo.GetAllIncluding(d => d.Taskitem);  // Assuming Tasks is a navigation property in Deal

            // Search functionality
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(d =>
                    d.DealName.Contains(input.Keyword) ||
                    d.Taskitem.Any(t => t.Title.Contains(input.Keyword) || t.Description.Contains(input.Keyword))
                );
            }

            // Sorting functionality (if provided)
            //if (!string.IsNullOrWhiteSpace(input.SortBy))
            //{
            //    if (input.SortBy == "DealName")
            //    {
            //        query = input.SortAscending ? query.OrderBy(d => d.DealName) : query.OrderByDescending(d => d.DealName);
            //    }
            //    else if (input.SortBy == "Date")
            //    {
            //        query = input.SortAscending ? query.OrderBy(d => d.Date) : query.OrderByDescending(d => d.Date);
            //    }
            //    // Add more sorting options as needed
            //}
            //else
            //{
            //    // Default sorting (e.g., by DealDate if not provided)
            //    query = query.OrderBy(d => d.Date);
            //}

            var totalCount = await query.CountAsync();  // Get total count for pagination

            var deals = await query
                .Skip(input.SkipCount)  // Pagination
                .Take(input.MaxResultCount)
                .ToListAsync();

            var dealDtos = deals.Select(deal => new DealDto
            {
                Id = deal.Id,
                DealName = deal.DealName,
                Date = deal.Date,
                Tasks = deal.Taskitem.Select(t => new TaskDto
                {
                    Id = t.Id,
                    TaskNo = t.TaskNo,
                    Title = t.Title,
                    DateFrom = t.DateFrom,
                    ToDate = t.ToDate,
                    Description = t.Description
                }).ToList()
            }).ToList();

            return new PagedResultDto<DealDto>(totalCount, dealDtos);
        }
        public async Task<DealDto> UpdateDealWithTasksAsync(int id, CreateUpdateDealDto input)
        {
            try
            {
                var deal = await _dealrepo.GetAsync(id);
                if (deal == null)
                {
                    throw new UserFriendlyException($"Deal with Id {id} not found.");
                }

                // Update deal properties
                deal.DealName = input.DealName;
                deal.Date = input.Date;

                await _dealrepo.UpdateAsync(deal);

                // Remove existing tasks
                var existingTasks = await _taskitemrepo.GetAllListAsync(t => t.DealId == id);
                foreach (var task in existingTasks)
                {
                    await _taskitemrepo.DeleteAsync(task);
                }

                // Add new tasks
                var taskDtos = new List<TaskDto>();
                foreach (var taskDto in input.Tasks)
                {
                    var newTask = new Task_Item
                    {
                        TaskNo = taskDto.TaskNo,
                        Title = taskDto.Title,
                        DateFrom = taskDto.DateFrom,
                        ToDate = taskDto.ToDate,
                        Description = taskDto.Description,
                        DealId = deal.Id
                    };

                    await _taskitemrepo.InsertAsync(newTask);

                    taskDtos.Add(new TaskDto
                    {
                        Id = newTask.Id,
                        TaskNo = newTask.TaskNo,
                        Title = newTask.Title,
                        DateFrom = newTask.DateFrom,
                        ToDate = newTask.ToDate,
                        Description = newTask.Description
                    });
                }

                return new DealDto
                {
                    Id = deal.Id,
                    DealName = deal.DealName,
                    Date = deal.Date,
                    Tasks = taskDtos
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}