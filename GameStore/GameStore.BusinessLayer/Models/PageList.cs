namespace GameStore.BusinessLayer.Models
{
    public class PageList<T>
    {
        public T Model { get; set; }
        public PageOptions PageOptions { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
    }
}