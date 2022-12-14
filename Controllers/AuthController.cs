using BTL_ASP_MVC.Data;
using BTL_ASP_MVC.Models;
using BTL_ASP_MVC.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BTL_ASP_MVC.Controllers
{
  public class AuthController : Controller
  {
    private readonly AppDbContext _context;

    private Encryptor _encryptor = new Encryptor();

    public AuthController(AppDbContext context)
    {
      _context = context;
    }

    public IActionResult Login()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(User user)
    {
      try
      {
        var result = await _context.Users.FirstOrDefaultAsync(
          u => u.username == user.username && u.password == _encryptor.MD5Hash(user.password));
        if (result != null)
        {
          HttpContext.Session.SetString("username", result.username);
          HttpContext.Session.SetString("userId", result.id);
          return RedirectToAction("Index", "Home");
        }
        else
        {
          ViewBag.error = "Username or password incorrect";
          return View();
        }
      }
      catch (System.Exception)
      {
        throw;
      }
    }
    public IActionResult Logout()
    {
      HttpContext.Session.Clear();
      return RedirectToAction(nameof(Login));
    }
  }
}