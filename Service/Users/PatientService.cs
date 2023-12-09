using AutoMapper;
using Core.Consts;
using Core.Domain.Bookings;
using Core.Domain.Users;
using Core.Enums;
using Core.Repository;
using Core.Service.Users;
using Core.Shared;
using Service.Dtos.Response.Doctors;
using Service.Dtos.Response.Patients;

namespace Service.Users
{
    public class PatientService : IPatientService
    {
        private readonly IUserService _userService;
        private readonly IPatientRepository _patientRepository;
        private readonly IMapper _mapper;

        public PatientService(IUserService userService, IPatientRepository patientRepository, IMapper mapper)
        {
            _userService = userService;
            _patientRepository = patientRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<User>>> GetAllPatientsAsync(int page, int pageSize, string search, string[]? includes = null)
        {
            IEnumerable<User> patients = await _patientRepository.FindAllPatientsAsync(page, pageSize, search);

            return Result.Success(patients);
        }

        public async Task<Result<dynamic>> GetPatientWithBookingsAsync(int id)
        {
            (User? patient, List<Booking> bookings) = await _patientRepository.FindPatientWithBookings(id);

            if (patient == null)
            {
                return Result.Failure<dynamic>(Error.Errors.Patients.PatientNotFound());
            }

            List<PatientRequestDto> requests = new List<PatientRequestDto>();

            bookings.ForEach(b =>
            {
                var request = new PatientRequestDto
                {
                    Day = b.AppointmentTime.Appointment.Day.ToString(),
                    DiscountCode = b.Coupon != null ? b.Coupon.Code : string.Empty,
                    DoctorName = b.Doctor.FullName,
                    ImageUrl = b.Doctor.ImageUrl,
                    Specialization = _mapper.Map<SpecializationDto>(b.Doctor.Specialization),
                    Price = b.Doctor.Price,
                    FinalPrice = b.Doctor.Price,
                    Status = b.Status.ToString(),
                    Time = b.AppointmentTime.Time
                };

                requests.Add(request);
            });

            var patientWithRequests = new GetPatientAndRequestsDto
            {
                Details = _mapper.Map<GetPatientDto>(patient),
                Requests = requests
            };

            return Result.Success<dynamic>(patientWithRequests);
        }

        public async Task<int> GetPatientsCountAsync()
        {
            int numberOfPatients = await _patientRepository.CountAsync(u => u.Discriminator == UserDiscriminator.Patient);

            return numberOfPatients;
        }

        public async Task<Result<bool>> RegisterPatientAsync(User user, string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return Result.Failure<bool>(Error.Errors.Users.PasswordNotMatched());
            }

            Result<User> registerPatientResult = await _userService.RegisterUserAsync(user, password, UserDiscriminator.Patient);

            if (registerPatientResult.IsFailure) return Result.Failure<bool>(registerPatientResult.Error);

            Result<bool> addPatientToRoleResult = await _userService.AddUserToRoleAsync(registerPatientResult.Value, AppConsts.Roles.Patient);

            if (addPatientToRoleResult.IsFailure) return Result.Failure<bool>(addPatientToRoleResult.Error);

            return Result.Success(true);
        }

    }
}
