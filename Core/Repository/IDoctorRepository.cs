using Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Users;
using Core.Enums;
using static Core.Consts.AppConsts;

namespace Core.Repository
{
    public interface IDoctorRepository : IBaseRepository<User>
    {
        Task<IEnumerable<User>> FindAllDoctorsAsync(int page, int pageSize, string search, string[]? includes = null);
    }
}
