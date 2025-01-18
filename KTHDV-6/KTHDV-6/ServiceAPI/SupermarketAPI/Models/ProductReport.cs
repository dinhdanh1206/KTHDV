namespace SupermarketAPI.Models
{
    public class ProductReport
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int TotalSold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Profit { get; set; }
        public DateTime ReportDate { get; set; }
    }
}