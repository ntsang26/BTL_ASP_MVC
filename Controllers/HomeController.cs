using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BTL_ASP_MVC.Models;
using BTL_ASP_MVC.Data;
using BTL_ASP_MVC.Utils;

namespace BTL_ASP_MVC.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;
  private readonly AppDbContext _context;

  private Encryptor encryptors = new Encryptor();
  public HomeController(ILogger<HomeController> logger, AppDbContext context)
  {
    _logger = logger;
    _context = context;
  }

  public IActionResult Index()
  {
    if (encryptors.CheckAuth(HttpContext))
      return View();
    return RedirectToAction("Login", "Auth");
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
