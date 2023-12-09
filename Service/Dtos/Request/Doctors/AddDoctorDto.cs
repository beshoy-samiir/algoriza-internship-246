using Service.Dtos.Request.Base;

namespace Service.Dtos.Request.Doctors
{
    public class AddDoctorDto : AddUserDto
    {
        public int SpecializationId { get; set; }
    }
}
