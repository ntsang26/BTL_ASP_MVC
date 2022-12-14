using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BTL_ASP_MVC.Data;
using BTL_ASP_MVC.Models;
using BTL_ASP_MVC.Utils;
using BTL_ASP_MVC.Models.Process;

namespace BTL_ASP_MVC.Controllers
{
  public class GiangVienController : Controller
  {
    private readonly AppDbContext _context;
    private Encryptor encryptors = new Encryptor();

    private ExcelProcess _excelProcess = new ExcelProcess();
    private StringProcess stringProcess = new StringProcess();

    public GiangVienController(AppDbContext context)
    {
      _context = context;
    }

    // GET: GiangVien
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

        var giangVien = from g in _context.GiangViens select g;
        if (giangVien != null)
        {
          if (!string.IsNullOrEmpty(search))
          {
            giangVien = giangVien.Where(x => x.name.ToLower().Contains(search.ToLower()));
          }
          return View(await PaginatedList<GiangVien>.CreateAsync(giangVien.AsNoTracking(), pageNumber ?? 1, 10));
        }
        return Problem("Entity set 'AppDbContext.GiangViens'  is null.");
      }
      return RedirectToAction("Login", "Auth");
    }

    public IActionResult Upload()
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        return View();
      }
      return RedirectToAction("Login", "Auth");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IFormFile file)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (file != null)
        {
          string fileExt = Path.GetExtension(file.FileName);
          if (fileExt != ".xls" && fileExt != ".xlsx")
          {
            ModelState.AddModelError("", "Please choose excel file!");
          }
          else
          {
            var fileName = DateTime.Now.ToShortTimeString() + fileExt;
            var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
            var fileLocation = new FileInfo(filePath).ToString();
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await file.CopyToAsync(stream);

              var dt = _excelProcess.ExcelToDataTable(fileLocation);
              for (int i = 0; i < dt.Rows.Count; i++)
              {
                var teacher = new GiangVien();
                teacher.name = dt.Rows[i][0].ToString();
                teacher.address = dt.Rows[i][1].ToString();
                teacher.phone = dt.Rows[i][2].ToString();
                teacher.gender = dt.Rows[i][3].ToString();
                teacher.birthday = Convert.ToDateTime(dt.Rows[i][4].ToString());

                _context.GiangViens.Add(teacher);
              }

              await _context.SaveChangesAsync();
              return RedirectToAction(nameof(Index));
            }
          }
        }
        return View();
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: GiangVien/Create
    public IActionResult Create()
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        return View();
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: GiangVien/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("giangvien_id,name,address,phone,gender,birthday,created_at,updated_at")] GiangVien giangVien)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (ModelState.IsValid)
        {
          _context.Add(giangVien);
          await _context.SaveChangesAsync();
          return RedirectToAction(nameof(Index));
        }
        return View(giangVien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: GiangVien/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.GiangViens == null)
        {
          return NotFound();
        }

        var giangVien = await _context.GiangViens.FindAsync(id);
        if (giangVien == null)
        {
          return NotFound();
        }
        return View(giangVien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: GiangVien/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("giangvien_id,name,address,phone,gender,birthday,created_at,updated_at")] GiangVien giangVien)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id != giangVien.giangvien_id)
        {
          return NotFound();
        }

        if (ModelState.IsValid)
        {
          try
          {
            _context.Update(giangVien);
            await _context.SaveChangesAsync();
          }
          catch (DbUpdateConcurrencyException)
          {
            if (!GiangVienExists(giangVien.giangvien_id))
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
        return View(giangVien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // GET: GiangVien/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (id == null || _context.GiangViens == null)
        {
          return NotFound();
        }

        var giangVien = await _context.GiangViens
            .FirstOrDefaultAsync(m => m.giangvien_id == id);
        if (giangVien == null)
        {
          return NotFound();
        }

        return View(giangVien);
      }
      return RedirectToAction("Login", "Auth");
    }

    // POST: GiangVien/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
      if (encryptors.CheckAuth(HttpContext))
      {
        if (_context.GiangViens == null)
        {
          return Problem("Entity set 'AppDbContext.GiangViens'  is null.");
        }
        var giangVien = await _context.GiangViens.FindAsync(id);
        if (giangVien != null)
        {
          _context.GiangViens.Remove(giangVien);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return RedirectToAction("Login", "Auth");
    }

    private bool GiangVienExists(string id)
    {
      return (_context.GiangViens?.Any(e => e.giangvien_id == id)).GetValueOrDefault();
    }
  }
}
