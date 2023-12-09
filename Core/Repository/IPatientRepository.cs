using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Bookings;
using Core.Domain.Users;
using static Core.Consts.AppConsts;

namespace Core.Repository
{
    public interface IPatientRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> FindAllPatientsAsync(int page, int pageSize, string search, string[]? includes = null);
        Task<(User?, List<Booking>)> FindPatientWithBookings(int id);
    }
}
