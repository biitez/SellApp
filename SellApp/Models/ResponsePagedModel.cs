using System;
using System.Collections.Generic;

namespace SellApp.Models
{
    public class ResponsePagedModel<T>
    {
        public List<T> data { get; set; }
        public Links links { get; set; }
        public Meta meta { get; set; }
    }

    public class Datum
    {
        public int id { get; set; }
        public string type { get; set; }
        public string data { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
    }

    public class Links
    {
        public string first { get; set; }
        public string last { get; set; }
        public object prev { get; set; }
        public object next { get; set; }
        public string url { get; set; }
        public string label { get; set; }
        public bool active { get; set; }
    }

    public class Meta
    {
        public int current_page { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public List<Links> links { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }
}
