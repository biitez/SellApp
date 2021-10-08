using System;
using System.Collections.Generic;
using System.Text;

namespace SellApp.Models.Coupons
{
    public class DataCouponModel
    {
        public string code { get; set; }
        public string type { get; set; }
        public string discount { get; set; }
        public int limit { get; set; }
        public bool store_wide { get; set; }
        public DateTime expires_at { get; set; }
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
        public string deleted_at { get; set; }
    }
}
