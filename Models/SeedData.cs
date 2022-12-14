using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BTL_ASP_MVC.Data;
using System;
using System.Linq;
using BTL_ASP_MVC.Utils;

namespace BTL_ASP_MVC.Models;

public static class SeedData
{
  public static void Initialize(IServiceProvider serviceProvider)
  {

    Encryptor encryptor = new Encryptor();
    using (var context = new AppDbContext(
        serviceProvider.GetRequiredService<
            DbContextOptions<AppDbContext>>()))
    {
      // Look for any movies.
      if (context.Users.Any())
      {
        return;   // DB has been seeded
      }
      context.Users.AddRange(
          new User
          {
            id = Guid.NewGuid().ToString("N"),
            username = "admin",
            password = encryptor.MD5Hash("admin"),
            fullname = "Admin",
            address = "HN",
            created_at = DateTime.Now,
            updated_at = DateTime.Now
          }
      );
      context.SaveChanges();
    }
  }
}