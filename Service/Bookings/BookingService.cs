using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Consts;
using Core.Domain.Bookings;
using Core.Repository;
using Core.Service.Bookings;
using Core.Shared;

namespace Service.Bookings
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }


        public async Task<List<int>> GetBookingsCountAsync()
        {
            var bookingsCount = await _bookingRepository.CountBookingRequestsAsync();

            var countOfCompleted = bookingsCount.FirstOrDefault(r => r.Status == AppConsts.General.Completed);
            var countOfPending = bookingsCount.FirstOrDefault(r => r.Status == AppConsts.General.Pending);
            var countOfCanceled = bookingsCount.FirstOrDefault(r => r.Status == AppConsts.General.Canceled);
            var countOfTotal = bookingsCount.FirstOrDefault(r => r.Status == AppConsts.General.Total);

            var result = new List<int>
            {
                countOfCompleted != null ? countOfCompleted.Count : 0,
                countOfPending != null ? countOfPending.Count : 0,
                countOfCanceled != null ? countOfCanceled.Count : 0,
                countOfTotal != null ? countOfTotal.Count : 0,
            };

            return result;
        }
    }
}
