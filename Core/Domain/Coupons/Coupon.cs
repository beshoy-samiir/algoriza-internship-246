using Core.Domain.Base;
using Core.Enums;
using Core.Domain.Base;
using Core.Enums;

namespace Core.Domain.Coupons
{
    public class Coupon : Entity<int>
    {
        public string Code { get; set; }
        public bool Active { get; set; }
        public DiscountType DiscountType { get; set; }
        public float Value { get; set; }
        public byte NumberOfRequests { get; set; }

        public void Update(Coupon newData)
        {
            Value = newData.Value;
            Code = newData.Code;
            DiscountType = newData.DiscountType;
            NumberOfRequests = newData.NumberOfRequests;
        }
    }
}
