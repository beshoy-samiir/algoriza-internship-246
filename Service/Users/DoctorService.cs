using Core.Consts;
using Core.Domain.Users;
using Core.Enums;
using Core.Repository;
using Core.Service.Users;
using Core.Shared;

namespace Service.Users
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserService _userService;

        public DoctorService(IDoctorRepository doctorRepository, IUserService userService, IBookingRepository bookingRepository)
        {
            _doctorRepository = doctorRepository;
            _userService = userService;
            _bookingRepository = bookingRepository;
        }

        public async Task<Result<IEnumerable<User>>> GetAllDoctorsAsync(int page, int pageSize, string search, string[]? includes = null)
        {
            IEnumerable<User> doctors = await _doctorRepository.FindAllDoctorsAsync(page, pageSize, search, includes);

            return Result.Success(doctors);
        }

        public async Task<Result<User>> GetDoctorAsync(int id, string[]? includes = null)
        {
            User? doctor = await _doctorRepository.FindAsync(d => d.Id == id && d.Discriminator == UserDiscriminator.Doctor, includes);

            if (doctor == null)
            {
                return Result.Failure<User>(Error.Errors.Doctors.DoctorNotFound());
            }

            return Result.Success(doctor);
        }

        public async Task<Result<int>> GetDoctorsCountAsync()
        {
            var doctorsCount = await _doctorRepository.CountAsync(u => u.Discriminator == UserDiscriminator.Doctor);

            return Result.Success(doctorsCount);
        }

        public async Task<Result<bool>> AddDoctorAsync(User user)
        {

            Result<User> registerDoctorResult = await _userService.RegisterUserAsync(user, AppConsts.User.DoctorBasePassword, UserDiscriminator.Doctor);

            if (registerDoctorResult.IsFailure) return Result.Failure<bool>(registerDoctorResult.Error);

            Result<bool> addDoctorToRoleResult = await _userService.AddUserToRoleAsync(registerDoctorResult.Value, AppConsts.Roles.Doctor);

            if (addDoctorToRoleResult.IsFailure) return Result.Failure<bool>(addDoctorToRoleResult.Error);

            return Result.Success(true);
        }

        public async Task<Result<bool>> UpdateDoctorAsync(User user)
        {
            var updateDoctorResult = await _userService.UpdateUserAsync(user);

            if (updateDoctorResult.IsFailure) return Result.Failure<bool>(updateDoctorResult.Error);

            return Result.Success(true);
        }

        public async Task<Result<bool>> DeleteDoctorAsync(int id)
        {
            bool doctorHasRequests = await _bookingRepository.AnyAsync(b => b.DoctorId == id);

            if (doctorHasRequests)
            {
                return Result.Failure<bool>(Error.Errors.Doctors.DoctorHasRequests());
            }

            var deleteDoctorResult = await _userService.DeleteUserAsync(id);

            if (deleteDoctorResult.IsFailure) return Result.Failure<bool>(deleteDoctorResult.Error);

            return Result.Success(true);
        }
    }
}
