namespace SupermarketAPI.Models
{
    public class OrderReport
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalProfit { get; set; }
        public DateTime ReportDate { get; set; }
        public List<ProductReport> ProductReports { get; set; }
    }
}