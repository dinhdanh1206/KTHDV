using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    [Table("orders")] // Ánh xạ lớp với bảng "orders"
    public class Order
    {
        [Key] // Đánh dấu là khóa chính
        [Column("id")] // Ánh xạ với cột "id"
        public int Id { get; set; }

        [Column("customer_name")] // Ánh xạ với cột "customer_name"
        public string CustomerName { get; set; }

        [Column("customer_email")] // Ánh xạ với cột "customer_email"
        public string CustomerEmail { get; set; }

        [Column("total_amount")] // Ánh xạ với cột "total_amount"
        public decimal TotalAmount { get; set; }

        [Column("status")] // Ánh xạ với cột "status"
        public string Status { get; set; }

        [Column("created_at")] // Ánh xạ với cột "created_at"
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")] // Ánh xạ với cột "updated_at"
        public DateTime UpdatedAt { get; set; }

        // Thiết lập mối quan hệ với OrderItems
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
