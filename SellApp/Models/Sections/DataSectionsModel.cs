using System;

namespace SellApp.Models.Sections
{
    public class DataSectionsModel
    {
        public string title { get; set; }
        public bool hidden { get; set; }
        public int order { get; set; }
        public string slug { get; set; }
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
    }
}
