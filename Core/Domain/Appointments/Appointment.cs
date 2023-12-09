using Core.Domain.Appointments;
using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Domain.Base;
using Core.Domain.Users;
using static Core.Consts.AppConsts;

namespace Core.Domain.Appointments
{
    public class Appointment : Entity<int>
    {
        public Days Day { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public User Doctor { get; set; }
        public virtual IEnumerable<AppointmentTime> Times { get; set; }
    }
}
