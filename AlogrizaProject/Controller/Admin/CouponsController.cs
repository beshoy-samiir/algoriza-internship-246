using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Core.Domain.Coupons;
using Core.Service.Settings;
using Core.Shared;
using Service.Dtos.Request.Coupons;
using Service.Dtos.Response.Coupons;

namespace Web.Controller.Admin
{
    public class CouponsController : SettingsController
    {
        private readonly IMapper _mapper;
        private readonly ICouponService _couponService;

        public CouponsController(IMapper mapper, ICouponService couponService)
        {
            _mapper = mapper;
            _couponService = couponService;
        }

        [HttpGet]
        public async Task<ActionResult<GetCouponDto>> Get(int id)
        {
            Result<Coupon> getCouponResult = await _couponService.GetCouponAsync(id);
            if (getCouponResult.IsFailure) return BadRequest(getCouponResult.Error);

            return Ok(_mapper.Map<GetCouponDto>(getCouponResult.Value));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCouponDto>>> GetAll()
        {
            Result<IEnumerable<Coupon>> getCouponsResult = await _couponService.GetAllCouponsAsync();
            if (getCouponsResult.IsFailure) return BadRequest(getCouponsResult.Error);

            return Ok(_mapper.Map<IEnumerable<GetCouponDto>>(getCouponsResult.Value));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> Add(AddCouponDto request)
        {
            Coupon coupon = _mapper.Map<Coupon>(request);

            Result<bool> addCouponResult = await _couponService.AddCouponAsync(coupon);

            if (addCouponResult.IsFailure) return BadRequest(addCouponResult.Error);

            return Ok(true);
        }

        [HttpPut]
        public async Task<ActionResult<bool>> Edit(EditCouponDto request)
        {
            Coupon coupon = _mapper.Map<Coupon>(request);

            Result<bool> editCouponResult = await _couponService.EditCouponAsync(coupon);

            if (editCouponResult.IsFailure) return BadRequest(editCouponResult.Error);

            return Ok(true);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            Result<bool> deleteCouponResult = await _couponService.DeleteCouponAsync(id);

            if (deleteCouponResult.IsFailure) return BadRequest(deleteCouponResult.Error);

            return Ok(true);
        }

        [HttpGet]
        public async Task<ActionResult<bool>> Deactivate(int id)
        {
            Result<bool> deactivateCouponResult = await _couponService.DeactivateCouponAsync(id);

            if (deactivateCouponResult.IsFailure) return BadRequest(deactivateCouponResult.Error);

            return Ok(true);
        }
    }
}
