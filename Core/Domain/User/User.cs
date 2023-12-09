using Core.Domain.Appointments;
using Core.Domain.Lookup;
using Core.Domain.Users.ValueObjects;
using Core.Enums;
using Core.Shared;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;


namespace Core.Domain.Users
{
    public class User : IdentityUser<int>
    {
        private const byte NameMinLength = 3;
        private const byte NameMaxLength = 32;

        public User()
        {
            UserName = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }

        public string FullName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public UserDiscriminator Discriminator { get; set; }

        [ForeignKey("Specialization")]
        public int? SpecializationId { get; set; } = null;
        public string? ImageUrl { get; set; }
        public float Price { get; set; }
        public virtual Specialization? Specialization { get; set; }
        public virtual IEnumerable<Appointment>? Appointments { get; set; }

        public static Result SetUserPassword(User user, string password)
        {
            user.PasswordHash = password;

            return Result.Success();
        }

        public void Update(User newData)
        {
            FirstName = newData.FirstName;
            LastName = newData.LastName;
            Email = newData.Email;
            PhoneNumber = newData.PhoneNumber;
            SpecializationId = newData.SpecializationId;
            Gender = newData.Gender;
            DateOfBirth = newData.DateOfBirth;

            if (!string.IsNullOrEmpty(newData.ImageUrl)) ImageUrl = newData.ImageUrl;
        }
    }
}
