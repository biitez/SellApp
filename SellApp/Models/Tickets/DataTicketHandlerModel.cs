using System;

namespace SellApp.Models.Tickets
{
    public class Customer
    {
        public string id { get; set; }
        public string email { get; set; }
    }

    public class DataTicketHandlerModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string status { get; set; }
        public Customer customer { get; set; }
        public object reference { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
        public int read_by { get; set; }
    }
}
