using Core.Domain.Coupons;
using Core.Repository;
using Core.Service.Settings;
using Core.Shared;

namespace Service.Settings
{
    public class CouponService : ICouponService
    {
        private readonly IBaseRepository<Coupon> _couponRepository;

        public CouponService(IBaseRepository<Coupon> couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<Result<IEnumerable<Coupon>>> GetAllCouponsAsync()
        {
            IEnumerable<Coupon> coupons = await _couponRepository.GetAllAsync();

            return Result.Success(coupons);
        }

        public async Task<Result<Coupon>> GetCouponAsync(int id)
        {
            Coupon? coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null) return Result.Failure<Coupon>(Error.Errors.Settings.CouponNotFound());

            return Result.Success(coupon);
        }

        public async Task<Result<bool>> AddCouponAsync(Coupon coupon)
        {
            bool couponAlreadyExist = await _couponRepository.AnyAsync(c => c.Code == coupon.Code);
            if (couponAlreadyExist) return Result.Failure<bool>(Error.Errors.Settings.CouponCodeAlreadyExist(coupon.Code));

            await _couponRepository.AddAsync(coupon);
            await _couponRepository.SaveChangesAsync();

            return Result.Success(true);
        }

        public async Task<Result<bool>> DeactivateCouponAsync(int id)
        {
            Coupon? coupon = await _couponRepository.GetByIdAsync(id);
            if (coupon == null) return Result.Failure<bool>(Error.Errors.Settings.CouponNotFound());

            coupon.Active = false;

            _couponRepository.Update(coupon);
            await _couponRepository.SaveChangesAsync();

            return Result.Success(true);
        }

        public async Task<Result<bool>> DeleteCouponAsync(int id)
        {
            bool deletedSuccessfully = await _couponRepository.DeleteAsync(id);
            if (!deletedSuccessfully) return Result.Failure<bool>(Error.Errors.Settings.CouponNotFound());

            await _couponRepository.SaveChangesAsync();

            return Result.Success(true);
        }

        public async Task<Result<bool>> EditCouponAsync(Coupon newCouponData)
        {
            Coupon? coupon = await _couponRepository.GetByIdAsync(newCouponData.Id);
            if (coupon == null) return Result.Failure<bool>(Error.Errors.Settings.CouponNotFound());

            coupon.Update(newCouponData);

            _couponRepository.Update(coupon);
            await _couponRepository.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
