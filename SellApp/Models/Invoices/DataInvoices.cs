using System;
using System.Collections.Generic;

namespace SellApp.Models.Invoices
{
    public class DataInvoices
    {
        public int id { get; set; }
        public Deliverable deliverable { get; set; }
        public Payment payment { get; set; }
        public CustomerInformation customer_information { get; set; }
        public List<object> additional_information { get; set; }
        public Status status { get; set; }
        public List<object> webhooks { get; set; }
        public string feedback { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
        public int coupon_id { get; set; }
        public int invoice_id { get; set; }
    }

    public class Deliverable
    {
        public int quantity { get; set; }
        public string type { get; set; }
        public List<object> data { get; set; }
    }

    public class Payment
    {
        public Subtotal subtotal { get; set; }
        public Gateway gateway { get; set; }
        public object data { get; set; }
    }

    public class Gateway
    {
        public string type { get; set; }
        public Fee fee { get; set; }
    }

    public class Fee
    {
        public string price { get; set; }
        public string currency { get; set; }
    }

    public class Total
    {
        public string exclusive { get; set; }
        public string inclusive { get; set; }
    }

    public class Subtotal
    {
        public int @base { get; set; }
        public string currency { get; set; }
        public int units { get; set; }
        public int vat { get; set; }
        public Total total { get; set; }
    }

    public class CustomerInformation
    {
        public string id { get; set; }
        public string email { get; set; }
        public string country { get; set; }
        public string Location { get; set; }
        public string IP { get; set; }
        public bool proxied { get; set; }
        public string browser_agent { get; set; }
    }

    public class Status
    {
        public string status { get; set; }
        public DateTime setAt { get; set; }
        public DateTime updatedAt { get; set; }
        public List<object> history { get; set; }
    }
}
