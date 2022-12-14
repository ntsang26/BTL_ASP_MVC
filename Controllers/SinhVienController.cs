using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL_ASP_MVC.Data;
using BTL_ASP_MVC.Models;
using BTL_ASP_MVC.Utils;

namespace BTL_ASP_MVC.Controllers
{
  public class SinhVienController : Controller
  {
    private readonly AppDbContext _context;
    private Encryptor encryptors = new Encryptor();


    public SinhVienController(AppDbContext context)
    {
      _context = context;
    }

    // GET: SinhVien
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

        var sinhVien = from g in _context.Sinhviens select g;
        if (sinhVien != null)
        {
          if (!string.IsNullOrEmpty(search))
          {
            sinhVien = sinhVien.Where(x => x.name.ToLower().Contains(search.ToLower()));
          }
          return View(await PaginatedList<Sinhvien>.CreateAsync(sinhVien.AsNoTracking().Include(s => s.Lop), pageNumber ?? 1, 10));
        }
        return Problem("Entity set 'AppDbContext.Sinhviens'  is null.");
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: SinhVien/Create
    public IActionResult Create()
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        ViewData["lop_id"] = new SelectList(_context.Lops, "lop_id", "name");
        return View();
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: SinhVien/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("sinhvien_id,name,address,phone,gender,birthday,lop_id,created_at,updated_at")] Sinhvien sinhvien)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (ModelState.IsValid)
        {
          _context.Add(sinhvien);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        ViewData["lop_id"] = new SelectList(_context.Lops, "lop_id", "name", sinhvien.lop_id);
        return View(sinhvien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: SinhVien/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.Sinhviens == null)
        {
          return NotFound();
        }

        var sinhvien = await _context.Sinhviens.FindAsync(id);
        if (sinhvien == null)
        {
          return NotFound();
        }
        ViewData["lop_id"] = new SelectList(_context.Lops, "lop_id", "name", sinhvien.lop_id);
        return View(sinhvien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: SinhVien/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("sinhvien_id,name,address,phone,gender,birthday,lop_id,created_at,updated_at")] Sinhvien sinhvien)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id != sinhvien.sinhvien_id)
        {
          return NotFound();
        }

        if (ModelState.IsValid)
        {
          try
          {
            _context.Update(sinhvien);
            await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
            if (!SinhvienExists(sinhvien.sinhvien_id))
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
        ViewData["lop_id"] = new SelectList(_context.Lops, "lop_id", "name", sinhvien.lop_id);
        return View(sinhvien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: SinhVien/Delete/5
    public async Task<IActionResult> Delete(string id)
    {

      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.Sinhviens == null)
        {
          return NotFound();
        }

        var sinhvien = await _context.Sinhviens
            .Include(s => s.Lop)
            .FirstOrDefaultAsync(m => m.sinhvien_id == id);
        if (sinhvien == null)
        {
          return NotFound();
        }

        return View(sinhvien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: SinhVien/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (_context.Sinhviens == null)
        {
          return Problem("Entity set 'AppDbContext.Sinhviens'  is null.");
        }
        var sinhvien = await _context.Sinhviens.FindAsync(id);
        if (sinhvien != null)
        {
          _context.Sinhviens.Remove(sinhvien);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return RedirectToAction("Login", "Auth");
    }

    private bool SinhvienExists(string id)
    {
      return (_context.Sinhviens?.Any(e => e.sinhvien_id == id)).GetValueOrDefault();
    }
  }
}
