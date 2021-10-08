using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SellApp.Models.Invoices.Session
{
    public partial class SessionDataInvoices
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("payment_url")]
        public Uri PaymentUrl { get; set; }

        [JsonProperty("invoice")]
        public Invoice Invoice { get; set; }
    }

    public partial class Invoice
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("deliverable")]
        public Deliverable Deliverable { get; set; }

        [JsonProperty("payment")]
        public Payment Payment { get; set; }

        [JsonProperty("customer_information")]
        public CustomerInformation CustomerInformation { get; set; }

        [JsonProperty("additional_information")]
        public List<object> AdditionalInformation { get; set; }

        [JsonProperty("status")]
        public InvoiceStatus Status { get; set; }

        [JsonProperty("webhooks")]
        public List<object> Webhooks { get; set; }

        [JsonProperty("feedback")]
        public string Feedback { get; set; }

        [JsonProperty("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("store_id")]
        public long StoreId { get; set; }

        [JsonProperty("coupon_id")]
        public long CouponId { get; set; }

        [JsonProperty("invoice_id")]
        public long InvoiceId { get; set; }
    }

    public partial class CustomerInformation
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("IP")]
        public string Ip { get; set; }

        [JsonProperty("proxied")]
        public bool Proxied { get; set; }

        [JsonProperty("browser_agent")]
        public string BrowserAgent { get; set; }
    }

    public partial class Deliverable
    {
        [JsonProperty("quantity")]
        public long Quantity { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public List<object> Data { get; set; }
    }

    public partial class Payment
    {
        [JsonProperty("subtotal")]
        public Subtotal Subtotal { get; set; }

        [JsonProperty("gateway")]
        public Gateway Gateway { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }

    public partial class Gateway
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("fee")]
        public Fee Fee { get; set; }
    }

    public partial class Fee
    {
        [JsonProperty("price")]
        public long Price { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }
    }

    public partial class Subtotal
    {
        [JsonProperty("base")]
        public long Base { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("units")]
        public long Units { get; set; }

        [JsonProperty("vat")]
        public long Vat { get; set; }

        [JsonProperty("total")]
        public Total Total { get; set; }
    }

    public partial class Total
    {
        [JsonProperty("exclusive")]
        public long Exclusive { get; set; }

        [JsonProperty("inclusive")]
        public long Inclusive { get; set; }
    }

    public partial class InvoiceStatus
    {
        [JsonProperty("status")]
        public StatusStatus Status { get; set; }

        [JsonProperty("history")]
        public List<object> History { get; set; }
    }

    public partial class StatusStatus
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("setAt")]
        public DateTimeOffset SetAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
