using Core.Enums;

namespace Service.Dtos.Request.Coupons
{
    public class AddCouponDto
    {
        public string Code { get; set; }
        public int NumberOfRequests { get; set; }
        public DiscountType DiscountType { get; set; }
        public float Value { get; set; }
    }
}
