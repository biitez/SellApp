using SellApp.Enums;
using System;

namespace SellApp.Models.Coupons
{
    public class NewCouponModel
    {
        /// <summary>
        /// The coupon code the customer enters during checkout.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// This can be either "PERCENTAGE" or "AMOUNT".
        /// </summary>
        public CouponTypes CouponType { get; set; }

        /// <summary>
        /// The discount value in percentage or cents.
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// Whether the coupon applies to all products within your store or not.
        /// </summary>
        public bool StoreWide { get; set; }

        /// <summary>
        /// (Optional) The maximum amount of times a coupon code can be used, across all customers.
        /// </summary>
        public int? UsesLimit { get; set; } = null;

        /// <summary>
        /// (Optional) The coupon's expiry date.
        /// </summary>
        public DateTime? ExpiresAt { get; set; } = null;
    }
}
