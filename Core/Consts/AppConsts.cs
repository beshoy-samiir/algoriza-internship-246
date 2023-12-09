using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Consts
{
    public static class AppConsts
    {
        public static class General
        {
            public const string Canceled = "Canceled";
            public const string Completed = "Completed";
            public const string Pending = "Pending";
            public const string Total = "Total";
        }

        public static class User
        {
            public const double TokenExpiryDays = 7;
            public const double RefreshTokenExpiryDays = 30;
            public const string DoctorBasePassword = "[%tw459r";
        }

        public static class Roles
        {
            public const string Admin = "Admin";
            public const string Doctor = "Doctor";
            public const string Patient = "Patient";
        }

        public static class DomainModels
        {
            public const string Specialization = "Specialization";
            public const string Appointments = "Appointments";
            public const string AppointmentTimes = "Times";
            public const string AppointmentsAndTimes = "Appointments.Times";
        }
    }
}
