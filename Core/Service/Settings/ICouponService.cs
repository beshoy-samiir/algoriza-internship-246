using Core.Shared;
using Core.Domain.Coupons;

namespace Core.Service.Settings
{
    public interface ICouponService
    {
        Task<Result<Coupon>> GetCouponAsync(int id);
        Task<Result<IEnumerable<Coupon>>> GetAllCouponsAsync();
        Task<Result<bool>> AddCouponAsync(Coupon coupon);
        Task<Result<bool>> EditCouponAsync(Coupon coupon);
        Task<Result<bool>> DeleteCouponAsync(int id);
        Task<Result<bool>> DeactivateCouponAsync(int id);
    }
}
