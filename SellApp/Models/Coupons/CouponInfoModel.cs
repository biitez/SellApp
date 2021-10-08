namespace SellApp.Models.Coupons
{
    public class CouponInfoModel
    {
        public CouponInfoData data { get; set; }
    }

    public class CouponInfoData
    {
        public string code { get; set; }
        public string type { get; set; }
        public string discount { get; set; }
        public int? limit { get; set; }
        public bool store_wide { get; set; }
        public string expires_at { get; set; }
        public int id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int store_id { get; set; }
        public string deleted_at { get; set; }
    }
}
