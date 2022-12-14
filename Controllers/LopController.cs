using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_ASP_MVC.Data;
using BTL_ASP_MVC.Models;
using BTL_ASP_MVC.Utils;
using BTL_ASP_MVC.Models.Process;

namespace BTL_ASP_MVC.Controllers
{
  public class LopController : Controller
  {
    private readonly AppDbContext _context;

    private Encryptor encryptors = new Encryptor();
    private ExcelProcess _excelProcess = new ExcelProcess();

    public LopController(AppDbContext context)
    {
      _context = context;
    }

    // GET: Lop
    public async Task<IActionResult> Index(string search, int? pageNumber, string currentSearch)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (search != null)
        {
          pageNumber = 1;
        }
        else
        {
          search = currentSearch;
        }
        ViewBag.currentSearch = search;

        var lop = from g in _context.Lops select g;
        if (lop != null)
        {
          if (!string.IsNullOrEmpty(search))
          {
            lop = lop.Where(x => x.name.ToLower().Contains(search.ToLower()));
          }
          return View(await PaginatedList<Lop>.CreateAsync(lop.AsNoTracking().Include(s => s.GiangVien), pageNumber ?? 1, 10));
        }
        return Problem("Entity set 'AppDbContext.Lops'  is null.");
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: Lop/Create
    public IActionResult Create()
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        ViewData["giangvien_id"] = new SelectList(_context.GiangViens, "giangvien_id", "name");
        return View();
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: Lop/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("lop_id,name,giangvien_id,created_at,updated_at")] Lop lop)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (ModelState.IsValid)
        {
          _context.Add(lop);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        ViewData["giangvien_id"] = new SelectList(_context.GiangViens, "giangvien_id", "name", lop.giangvien_id);
        return View(lop);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: Lop/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.Lops == null)
        {
          return NotFound();
        }

        var lop = await _context.Lops.FindAsync(id);
        if (lop == null)
        {
          return NotFound();
        }
        ViewData["giangvien_id"] = new SelectList(_context.GiangViens, "giangvien_id", "name", lop.giangvien_id);
        return View(lop);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: Lop/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("lop_id,name,giangvien_id,created_at,updated_at")] Lop lop)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id != lop.lop_id)
        {
          return NotFound();
        }

        if (ModelState.IsValid)
        {
          try
          {
            _context.Update(lop);
            await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
            if (!LopExists(lop.lop_id))
            {
              return NotFound();
            }
            else
            {
              throw;
            }
          }
          return RedirectToAction(nameof(Index));
        }
        ViewData["giangvien_id"] = new SelectList(_context.GiangViens, "giangvien_id", "name", lop.giangvien_id);
        return View(lop);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: Lop/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.Lops == null)
        {
          return NotFound();
        }

        var lop = await _context.Lops
            .Include(l => l.GiangVien)
            .FirstOrDefaultAsync(m => m.lop_id == id);
        if (lop == null)
        {
          return NotFound();
        }

        return View(lop);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: Lop/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (_context.Lops == null)
        {
          return Problem("Entity set 'AppDbContext.Lops'  is null.");
        }
        var lop = await _context.Lops.FindAsync(id);
        if (lop != null)
        {
          _context.Lops.Remove(lop);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return RedirectToAction("Login", "Auth");
    }

    private bool LopExists(string id)
    {
      return (_context.Lops?.Any(e => e.lop_id == id)).GetValueOrDefault();
    }
  }
}
