using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_BoilerPlate.Admissions.Dto
{
    public class GetAdmissionDto
    {
        public int Id { get; set; }
        public int PatientId { get; set; }

        public string PatientName { get; set; }
        public int BedId { get; set; }
        public string BedNumber { get; set; }

        public DateTime AdmitDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string Notes { get; set; }

    }
}
