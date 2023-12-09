using Service.Dtos.Response.Admin;

namespace Service.Dtos.Response.Admin
{
    public class TopSpecializationResponseDto
    {
        public string FullName { get; set; }
        public NumberOfRequestsResponseDto Requests { get; set; }
    }
}
