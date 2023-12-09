using Service.Dtos.Response.Base;

namespace Service.Dtos.Response.Doctors
{
    public class GetDoctorDto : GetUserDto
    {
        public SpecializationDto Specialization { get; set; }
    }

    public class SpecializationDto
    {
        public string Name { get; set; }
    }
}
