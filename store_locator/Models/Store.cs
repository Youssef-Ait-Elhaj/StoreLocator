namespace store_locator.Models
{
    public class Store
    {
        private StoreContext context;
        public int id { get; set; }
        public string address {get; set; }
        public double longtitude { get; set; }
        public double latitude { get; set; }
    }
}