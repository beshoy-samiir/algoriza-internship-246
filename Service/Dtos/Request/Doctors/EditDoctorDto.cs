using Service.Dtos.Request.Doctors;

namespace Service.Dtos.Request.Doctors
{
    public class EditDoctorDto : AddDoctorDto
    {
        public int Id { get; set; }
    }
}