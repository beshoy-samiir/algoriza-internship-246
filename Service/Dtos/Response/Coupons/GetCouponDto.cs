using Core.Enums;

namespace Service.Dtos.Response.Coupons
{
    public class GetCouponDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public DiscountType DiscountType { get; set; }
        public float Value { get; set; }
        public int NumberOfRequests { get; set; }
        public bool Active { get; set; }
    }
}
