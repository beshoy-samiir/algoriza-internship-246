using Microsoft.AspNetCore.Http;
using Core.Enums;

namespace Service.Dtos.Request.Base
{
    public class AddUserDto
    {
        public IFormFile? Image { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
    }
}
