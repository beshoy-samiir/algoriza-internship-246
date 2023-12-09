using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Enums;

namespace Service.Dtos.Response.Appointments
{
    public class GetAppointmentDto
    {
        public Days Day { get; set; }
        public IEnumerable<AppointmentScheduleDto> Times { get; set; }
    }

    public class AppointmentScheduleDto
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool Booked { get; set; }
    }
}
