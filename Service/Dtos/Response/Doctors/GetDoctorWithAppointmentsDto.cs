using Service.Dtos.Response.Appointments;
using Service.Dtos.Response.Doctors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Response.Doctors
{
    public class GetDoctorWithAppointmentsDto : GetDoctorDto
    {
        public IEnumerable<GetAppointmentDto> Appointments { get; set; }
    }
}
