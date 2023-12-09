using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Consts;
using Core.Domain.Users;
using Core.Service.Users;
using Core.Shared;
using Service.Dtos.Request.Doctors;
using Service.Dtos.Request.Patients;
using Service.Dtos.Response.Doctors;
using Web.Helpers;


namespace Web.Controller.Patient
{
    public class PatientController : ApplicationController
    {
        private readonly IImageHelper _imageHelper;
        private readonly IPatientService _patientService;
        private readonly IUserService _userService;
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;

        public PatientController(IPatientService patientService, IImageHelper imageHelper, IUserService userService, IDoctorService doctorService, IMapper mapper)
        {
            _patientService = patientService;
            _imageHelper = imageHelper;
            _userService = userService;
            _doctorService = doctorService;
            _mapper = mapper;
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<ActionResult<bool>> Register([FromForm] RegisterPatientDto request)
        {
            string imageUrl = "";

            if (request.Image != null)
            {
                Result<string> UploadImageResult = _imageHelper.UploadImage(request.Image);
                if (UploadImageResult.IsFailure) return BadRequest(UploadImageResult.Error);

                imageUrl = UploadImageResult.Value;
            }

            User user = _mapper.Map<User>(request);
            user.ImageUrl = imageUrl;

            Result<bool> registerPatientResult = await _patientService.RegisterPatientAsync(user, request.Password, request.ConfirmPassword);

            if (registerPatientResult.IsFailure) return BadRequest(registerPatientResult.Error);

            return Ok(true);
        }

        [HttpPost("api/[controller]/[action]")]
        public async Task<ActionResult<UserJwtToken>> Login(LoginPatientDto request)
        {
            Result<UserJwtToken> loginResult = await _userService.LoginUserAsync(request.Email, request.Password);

            if (loginResult.IsFailure) return BadRequest(loginResult.Error);

            return Ok(loginResult.Value);
        }

        [HttpGet("api/[controller]/doctors/[action]")]
        //[Authorize(Roles = AppConsts.Roles.Patient)]
        public async Task<ActionResult<IEnumerable<GetDoctorWithAppointmentsDto>>> GetAll(int page, int pageSize, string search)
        {
            var doctors = await _doctorService.GetAllDoctorsAsync(
                page,
                pageSize,
                search,
                new[] { AppConsts.DomainModels.Specialization, AppConsts.DomainModels.AppointmentsAndTimes });

            return Ok(_mapper.Map<IEnumerable<GetDoctorWithAppointmentsDto>>(doctors));
        }
    }
}
