using Service.Dtos.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Response.Patients
{
    public class GetPatientDto : GetUserDto
    {
        public string Email { get; set; }
    }
}
