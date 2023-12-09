using Core.Shared;
using Core.Domain.Users;
using static Core.Consts.AppConsts;

namespace Core.Service.Users
{
    public interface IPatientService
    {
        Task<Result<IEnumerable<User>>> GetAllPatientsAsync(int page, int pageSize, string search, string[]? includes = null);
        Task<Result<dynamic>> GetPatientWithBookingsAsync(int id);
        Task<int> GetPatientsCountAsync();
        Task<Result<bool>> RegisterPatientAsync(User user, string password, string confirmPassword);

    }
}
