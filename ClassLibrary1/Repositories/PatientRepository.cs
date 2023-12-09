using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Core.Domain.Bookings;
using Core.Domain.Users;
using Core.Enums;
using Core.Repository;

namespace Repository.Repositories
{
    public class PatientRepository : BaseRepository<User>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> FindAllPatientsAsync(int page, int pageSize, string search, string[]? includes = null)
        {
            Expression<Func<User, bool>> exp = (u) => u.Discriminator == UserDiscriminator.Patient;

            if (!string.IsNullOrEmpty(search)) exp = (u) => u.Discriminator == UserDiscriminator.Patient && u.FullName.Contains(search);

            IEnumerable<User> patients = await FindAllAsync(exp, page, pageSize, includes);

            return patients;
        }

        public async Task<(User?, List<Booking>)> FindPatientWithBookings(int id)
        {
            List<Booking> bookings = new List<Booking>();
            User? patient = await FindAsync(d => d.Id == id && d.Discriminator == UserDiscriminator.Patient);

            if (patient != null)
            {
                bookings = await _context.Bookings
                    .Where(b => b.PatientId == id)
                    .Include(b => b.Doctor)
                    .ThenInclude(b => b.Specialization)
                    .Include(b => b.Coupon)
                    .Include(b => b.AppointmentTime)
                    .ThenInclude(b => b.Appointment)
                    .ToListAsync();
            }

            return (patient, bookings);
        }
    }
}
