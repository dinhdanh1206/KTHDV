using Microsoft.EntityFrameworkCore;
using ReportingAPI.Models;

namespace ReportingAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrderReport> OrderReports { get; set; }
        public DbSet<ProductReport> ProductReports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderReport>(entity =>
            {
                entity.ToTable("orders_reports");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.OrderId).HasColumnName("order_id");
                entity.Property(e => e.TotalRevenue).HasColumnName("total_revenue");
                entity.Property(e => e.TotalCost).HasColumnName("total_cost");
                entity.Property(e => e.TotalProfit).HasColumnName("total_profit");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<ProductReport>(entity =>
            {
                entity.ToTable("product_reports");
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.OrderReportId).HasColumnName("order_report_id");
                entity.Property(e => e.ProductId).HasColumnName("product_id");
                entity.Property(e => e.TotalSold).HasColumnName("total_sold");
                entity.Property(e => e.Revenue).HasColumnName("revenue");
                entity.Property(e => e.Cost).HasColumnName("cost");
                entity.Property(e => e.Profit).HasColumnName("profit");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne<OrderReport>()
                    .WithMany()
                    .HasForeignKey(e => e.OrderReportId);
            });
        }
    }
}