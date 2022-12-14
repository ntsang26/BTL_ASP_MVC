using Microsoft.EntityFrameworkCore;

namespace BTL_ASP_MVC.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<BTL_ASP_MVC.Models.User> Users { get; set; }
    public DbSet<BTL_ASP_MVC.Models.Sinhvien> Sinhviens { get; set; }
    public DbSet<BTL_ASP_MVC.Models.Lop> Lops { get; set; } = default!;
    public DbSet<BTL_ASP_MVC.Models.GiangVien> GiangViens { get; set; } = default!;
  }
}