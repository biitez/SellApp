using System;

namespace SellApp.Models.Tickets
{
    public class DataTicketMessagesManagerModel
    {
        public int id { get; set; }
        public string author { get; set; }
        public string content { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public int ticket_id { get; set; }
    }
}
