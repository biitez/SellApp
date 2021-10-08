using System;
using System.Collections.Generic;

namespace SellApp.Models.Listings
{
    public class Metadata
    {
        public int size { get; set; }
        public string filename { get; set; }
        public string extension { get; set; }
        public string mime_type { get; set; }
    }

    public class Image
    {
        public string path { get; set; }
        public Metadata metadata { get; set; }
    }

    public class Data
    {
        public int stock { get; set; }
        public object file { get; set; }
        public object serials { get; set; }
        public object removeDuplicate { get; set; }
        public object webhook { get; set; }
        public string comment { get; set; }
        public string title { get; set; }
        public string slug { get; set; }
        public string description { get; set; }
        public Image image { get; set; }
        public int order { get; set; }
        public string visibility { get; set; }
        public Deliverable deliverable { get; set; }
        public Price price { get; set; }
        public bool humble { get; set; }
        public List<string> payment_methods { get; set; }
        public List<object> additional_information { get; set; }
        public List<object> bulk_discount { get; set; }
        public int? minimum_purchase_quantity { get; set; }
        public int? maximum_purchase_quantity { get; set; }
        public bool? locked { get; set; }
        public int id { get; set; }
        public string deleted_at { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int store_id { get; set; }
        public int? category_id { get; set; }
        public int? section_id { get; set; }
        public int? section_order { get; set; }
    }

    public class Deliverable
    {
        public string delivery_text { get; set; }
        public string type { get; set; }
        public Data data { get; set; }
    }

    public class Price
    {
        /// <summary>
        /// The value of the product, if you want to see it with 
        /// decimals, call to the extension `PriceToDecimals()`
        /// </summary>
        public int price { get; set; }

        public string currency { get; set; }
    }

    public class Links
    {
        public string first { get; set; }
        public string last { get; set; }
        public string prev { get; set; }
        public string next { get; set; }
    }

    public class Meta
    {
        public int current_page { get; set; }
        public int from { get; set; }
        public int last_page { get; set; }
        public string path { get; set; }
        public int per_page { get; set; }
        public int to { get; set; }
        public int total { get; set; }
    }

    public class ResponsePagedListingsModel
    {
        public List<Data> data { get; set; }
        public Links links { get; set; }
        public Meta meta { get; set; }
    }
}
