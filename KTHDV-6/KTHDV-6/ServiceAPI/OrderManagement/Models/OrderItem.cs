using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    [Table("order_items")] // Ánh xạ với bảng "order_items"
    public class OrderItem
    {
        [Key] // Đánh dấu đây là khóa chính
        [Column("id")] // Ánh xạ với cột "id"
        public int Id { get; set; }

        [Column("order_id")] // Ánh xạ với cột "order_id"
        public int OrderId { get; set; }

        [Column("product_id")] // Ánh xạ với cột "product_id"
        public int ProductId { get; set; }

        [Column("product_name")] // Ánh xạ với cột "product_name"
        public string ProductName { get; set; }

        [Column("quantity")] // Ánh xạ với cột "quantity"
        public int Quantity { get; set; }

        [Column("unit_price")] // Ánh xạ với cột "unit_price"
        public decimal UnitPrice { get; set; }

        [Column("total_price")] // Ánh xạ với cột "total_price"
        public decimal TotalPrice { get; set; }
    }
}
