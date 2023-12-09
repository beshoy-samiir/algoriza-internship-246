using Microsoft.AspNetCore.Mvc;
using Core.Service.Bookings;
using Core.Service.Users;
using Service.Dtos.Response.Admin;

namespace Web.Controller.Admin
{
    [Route("api/admin/dashboard/[action]")]
    public class AdminDashboardController : ApplicationController
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IBookingService _bookingService;

        public AdminDashboardController(IDoctorService doctorService, IPatientService patientService, IBookingService bookingService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _bookingService = bookingService;
        }


        [HttpGet]
        public async Task<ActionResult<int>> GetNumberOfDoctors()
        {
            var getNumberOfDoctorsResult = await _doctorService.GetDoctorsCountAsync();

            if (getNumberOfDoctorsResult.IsFailure)
            {
                return BadRequest(getNumberOfDoctorsResult.Error);
            }

            return Ok(getNumberOfDoctorsResult.Value);
        }

        [HttpGet]
        public async Task<ActionResult<int>> GetNumberOfPatients()
        {
            int numberOfPatients = await _patientService.GetPatientsCountAsync();
            return Ok(numberOfPatients);
        }


        [HttpGet]
        public async Task<ActionResult<NumberOfRequestsResponseDto>> GetNumberOfRequests()
        {
            var bookingsCountList = await _bookingService.GetBookingsCountAsync();

            var response = new NumberOfRequestsResponseDto
            {
                CompletedRequests = bookingsCountList[0],
                PendingRequests = bookingsCountList[1],
                CancelledRequests = bookingsCountList[2],
                TotalRequests = bookingsCountList[3],
            };

            return Ok(response);
        }

        [HttpGet]
        public ActionResult TopFiveSpecializations()
        {
            return Ok("hello");
        }

        [HttpGet]
        public ActionResult TopTenDoctors()
        {
            return Ok("hello");
        }
    }
}
