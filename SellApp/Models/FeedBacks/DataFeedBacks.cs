using System;

namespace SellApp.Models.FeedBacks
{
    public class DataFeedBacks
    {
        public string message { get; set; }
        public string reply { get; set; }
        public string feedback { get; set; }
        public int rating { get; set; }
        public int listing_id { get; set; }
        public int invoice_id { get; set; }
        public int id { get; set; }
        public string deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
    }
}
