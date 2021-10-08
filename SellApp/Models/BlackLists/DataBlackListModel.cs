using System;

namespace SellApp.Models.BlackLists
{
    public class DataBlackListModel
    {
        public string type { get; set; }
        public string data { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
    }
}
