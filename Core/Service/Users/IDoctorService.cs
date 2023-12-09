using Core.Shared;
using Core.Domain.Users;
using static Core.Consts.AppConsts;

namespace Core.Service.Users
{
    public interface IDoctorService
    {
        Task<Result<int>> GetDoctorsCountAsync();
        Task<Result<IEnumerable<User>>> GetAllDoctorsAsync(int page, int pageSize, string search, string[]? includes = null);
        Task<Result<User>> GetDoctorAsync(int id, string[]? includes = null);
        Task<Result<bool>> AddDoctorAsync(User user);
        Task<Result<bool>> UpdateDoctorAsync(User user);
        Task<Result<bool>> DeleteDoctorAsync(int id);
    }
}
