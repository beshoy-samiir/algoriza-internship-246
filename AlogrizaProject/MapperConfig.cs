using AutoMapper;
using Vezeeta.Core.Domain.Appointments;
using Vezeeta.Core.Domain.Coupons;
using Vezeeta.Core.Domain.Lookup;
using Vezeeta.Core.Domain.Users;
using Vezeeta.Service.Dtos.Request.Coupons;
using Vezeeta.Service.Dtos.Request.Doctors;
using Vezeeta.Service.Dtos.Request.Patients;
using Vezeeta.Service.Dtos.Response.Appointments;
using Vezeeta.Service.Dtos.Response.Coupons;
using Vezeeta.Service.Dtos.Response.Doctors;
using Vezeeta.Service.Dtos.Response.Patients;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vezeeta.Web
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, AddDoctorDto>().ReverseMap();
            CreateMap<User, EditDoctorDto>().ReverseMap();
            CreateMap<User, GetDoctorDto>().ReverseMap();
            CreateMap<User, GetDoctorWithAppointmentsDto>().ReverseMap();
            CreateMap<User, RegisterPatientDto>().ReverseMap();
            CreateMap<User, GetPatientDto>().ReverseMap();
            CreateMap<Appointment, GetAppointmentDto>().ReverseMap();
            CreateMap<AppointmentTime, AppointmentScheduleDto>().ReverseMap();
            CreateMap<Specialization, SpecializationDto>().ReverseMap();
            CreateMap<Coupon, AddCouponDto>().ReverseMap();
            CreateMap<Coupon, EditCouponDto>().ReverseMap();
            CreateMap<Coupon, GetCouponDto>().ReverseMap();
        }
    }
}