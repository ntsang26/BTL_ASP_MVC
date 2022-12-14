using System.ComponentModel.DataAnnotations;

namespace BTL_ASP_MVC.Models
{
  public class GiangVien
  {
    [Key]
    public string giangvien_id { get; set; } = Guid.NewGuid().ToString("N");
    public string name { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime birthday { get; set; }
    public DateTime created_at { get; set; } = DateTime.Now;
    public DateTime updated_at { get; set; } = DateTime.Now;
  }
}