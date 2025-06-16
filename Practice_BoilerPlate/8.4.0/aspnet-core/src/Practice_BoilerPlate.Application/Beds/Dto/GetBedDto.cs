using Abp.Domain.Entities;
using BoilerPlate_New.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoilerPlate_New.Beds.Dto
{
    public class GetBedDto: Entity<int>
    {
        public int Id { get; set; }

        public string BedNumber { get; set; }

        public BedType Type { get; set; }

        public BedStatus Status { get; set; }
    }
}
