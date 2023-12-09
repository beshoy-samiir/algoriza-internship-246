using Service.Dtos.Request.Coupons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dtos.Request.Coupons
{
    public class EditCouponDto : AddCouponDto
    {
        public int Id { get; set; }
    }
}
