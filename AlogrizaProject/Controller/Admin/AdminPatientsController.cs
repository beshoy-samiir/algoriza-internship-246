using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Consts;
using Core.Service.Users;
using Service.Dtos.Response.Patients;
using Web.Controller;

namespace Web.Controller.Admin
{
    [Route("api/admin/patients/[action]")]
    public class AdminPatientsController : ApplicationController
    {
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public AdminPatientsController(IPatientService patientService, IMapper mapper)
        {
            _patientService = patientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetPatientDto>>> GetAll(int page = 1, int pageSize = 10, string search = "")
        {
            var getPatientsResult = await _patientService.GetAllPatientsAsync(page, pageSize, search);

            if (getPatientsResult.IsFailure)
            {
                return BadRequest(getPatientsResult.Error);
            }

            var response = _mapper.Map<IEnumerable<GetPatientDto>>(getPatientsResult.Value);

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<GetPatientDto>> GetById(int id)
        {
            var getPatientResult = await _patientService.GetPatientWithBookingsAsync(id);

            if (getPatientResult.IsFailure)
            {
                return BadRequest(getPatientResult.Error);
            }

            var response = getPatientResult.Value;

            return Ok(response);
        }
    }
}
