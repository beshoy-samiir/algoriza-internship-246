using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service.Bookings
{
    public interface IBookingService
    {
        Task<List<int>> GetBookingsCountAsync();
    }
}
