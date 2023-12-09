using Core.Repository;
using Core.Shared;
using Core.Domain.Bookings;

namespace Core.Repository
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<List<RequestCount>> CountBookingRequestsAsync();
    }
}
