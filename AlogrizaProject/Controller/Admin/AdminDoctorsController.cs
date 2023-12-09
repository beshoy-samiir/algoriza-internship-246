using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Consts;
using Core.Domain.Users;
using Core.Service.Users;
using Core.Shared;
using Service.Dtos.Request.Doctors;
using Service.Dtos.Response.Doctors;
using Web.Helpers;

namespace Web.Controller.Admin
{
    [Route("api/admin/doctors/[action]")]
    public class AdminDoctorsController : ApplicationController
    {
        private readonly IDoctorService _doctorService;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;


        public AdminDoctorsController(IDoctorService doctorService, IMapper mapper, IImageHelper imageHelper)
        {
            _doctorService = doctorService;
            _mapper = mapper;
            _imageHelper = imageHelper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDoctorDto>>> GetAll(int page = 1, int pageSize = 10, string search = "")
        {
            var getDoctorsResult = await _doctorService.GetAllDoctorsAsync(page, pageSize, search, new[] { AppConsts.DomainModels.Specialization });

            if (getDoctorsResult.IsFailure)
            {
                return BadRequest(getDoctorsResult.Error);
            }

            var response = _mapper.Map<IEnumerable<GetDoctorDto>>(getDoctorsResult.Value);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<GetDoctorDto>> GetById(int id)
        {
            var getDoctorResult = await _doctorService.GetDoctorAsync(id, new[] { AppConsts.DomainModels.Specialization });

            if (getDoctorResult.IsFailure)
            {
                return BadRequest(getDoctorResult.Error);
            }

            var response = _mapper.Map<GetDoctorDto>(getDoctorResult.Value);

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Add([FromForm] AddDoctorDto request)
        {
            User user = _mapper.Map<User>(request);

            Result<string> uploadImageResult = _imageHelper.UploadImage(request.Image);

            if (uploadImageResult.IsFailure) return BadRequest(uploadImageResult.Error);

            user.ImageUrl = uploadImageResult.Value;

            Result<bool> AddDoctorResult = await _doctorService.AddDoctorAsync(user);

            if (AddDoctorResult.IsFailure) return BadRequest(AddDoctorResult.Error);

            return Ok(true);
        }


        [HttpPut]
        public async Task<ActionResult<bool>> Edit([FromForm] EditDoctorDto request)
        {
            User user = _mapper.Map<User>(request);

            if (request.Image != null)
            {
                Result<string> uploadImageResult = _imageHelper.UploadImage(request.Image);

                if (uploadImageResult.IsFailure) return BadRequest(uploadImageResult.Error);

                user.ImageUrl = uploadImageResult.Value;
            }

            Result<bool> AddDoctorResult = await _doctorService.UpdateDoctorAsync(user);

            if (AddDoctorResult.IsFailure) return BadRequest(AddDoctorResult.Error);

            return Ok(true);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            Result<bool> deleteDoctorResult = await _doctorService.DeleteDoctorAsync(id);

            if (deleteDoctorResult.IsFailure) return BadRequest(deleteDoctorResult.Error);

            return Ok(true);
        }
    }
}
