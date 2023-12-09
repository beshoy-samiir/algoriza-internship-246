using Service.Dtos.Request.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Request.Patients
{
    public class RegisterPatientDto : AddUserDto
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
