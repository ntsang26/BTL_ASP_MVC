using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTL_ASP_MVC.Models
{
  public class Sinhvien
  {
    [Key]
    public string sinhvien_id { get; set; } = Guid.NewGuid().ToString("N");
    public string name { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime birthday { get; set; }
    public string lop_id { get; set; }
    [ForeignKey("lop_id")]
    public Lop? Lop { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
    public DateTime updated_at { get; set; } = DateTime.Now;

  }
}