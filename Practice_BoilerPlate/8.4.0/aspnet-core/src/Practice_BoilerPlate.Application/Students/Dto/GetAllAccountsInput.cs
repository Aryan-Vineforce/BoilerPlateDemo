using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Students.Dto
{
    public class GetAllAccountsInput: PagedAndSortedResultRequestDto

    {
        public string Keyword { get; set; }

   
    }
}
