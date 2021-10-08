 namespace SellApp.Models
{
    public class PagesModel<T>
    {
        public T BL { get; set; }

        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
