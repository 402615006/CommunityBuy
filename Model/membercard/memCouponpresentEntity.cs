using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommunityBuy.CommonBasic;
namespace CommunityBuy.Model
{
    public class memCouponpresentEntity
    {
        public string status = "0";
        public string mes = string.Empty;
        public List<couponpresentEntity> data = new List<couponpresentEntity>();
    }
}