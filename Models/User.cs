using System.ComponentModel.DataAnnotations;

namespace BTL_ASP_MVC.Models
{
  public class User
  {
    [Key]
    public string id { get; set; } = Guid.NewGuid().ToString("N");
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string fullname { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public DateTime created_at { get; set; } = DateTime.Now;
    public DateTime updated_at { get; set; } = DateTime.Now;

  }
}