using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASP_MVC.Models
{
  public class Lop
  {
    [Key]
    public string lop_id { get; set; } = Guid.NewGuid().ToString("N");
    public string name { get; set; } = string.Empty;
    public string giangvien_id { get; set; }
    [ForeignKey("giangvien_id")]
    public GiangVien? GiangVien { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
    public DateTime updated_at { get; set; } = DateTime.Now;
  }
}