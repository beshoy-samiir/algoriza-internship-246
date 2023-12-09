using Core.Domain.Appointments;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Base;

namespace Core.Domain.Appointments
{
    public class AppointmentTime : Entity<int>
    {

        [ForeignKey("Appointment")]
        public int AppointmentId { get; set; }
        public DateTime Time { get; set; }
        public bool Booked { get; set; }
        public Appointment Appointment { get; set; }
    }
}
