using BulkyWeb.Domain.Entities;
using BulkyWeb.Infrastructure.Data;
using BulkyWeb.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Controllers;

public class VillaNumberController : Controller
{
  private readonly ApplicationDbContext _context;

  public VillaNumberController(ApplicationDbContext context)
  {
    _context = context;
  }

  public IActionResult Index()
  {
    // load the navigation property and then display it
    var villaNumbers = _context.VillaNumbers.Include(u=>u.Villa).ToList();
    return View(villaNumbers);
  }

  public IActionResult Create()
  {
    //// projection
    //IEnumerable<SelectListItem> list = _context.Villas.ToList().Select(u => new SelectListItem
    //{
    //  Text = u.Name,
    //  Value = u.Id.ToString()
    //});
    //ViewData["VillaList"] = list;
    //// ViewBag for the dynamic type
    //ViewBag.VillaList = list;

    VillaNumberVM vm = new()
    {
      VillaList = _context.Villas.ToList().Select(u => new SelectListItem
      {
        Text = u.Name,
        Value = u.Id.ToString()
      }),
    };

    return View(vm);
  }

  [HttpPost]
  public IActionResult Create(VillaNumberVM obj)
  {
    if (ModelState.IsValid)
    {
      _context.VillaNumbers.Add(obj.VillaNumber);
      _context.SaveChanges();

      TempData["success"] = "create success!";

      return RedirectToAction("Index");
    }
    TempData["error"] = "create failed!";
    return View();
  }

  public IActionResult Update(int villa_Number)
  {
    var obj = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villa_Number);


    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Update(VillaNumber obj)
  {
    if (ModelState.IsValid && obj.Villa_Number > 0)
    {
      _context.VillaNumbers.Update(obj);
      _context.SaveChanges();

      TempData["success"] = "update success!";

      return RedirectToAction("Index");
    }

    TempData["error"] = "update failed!";
    return View();
  }

  public IActionResult Delete(int villa_Number)
  {
    var obj = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villa_Number);


    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Delete(VillaNumber obj)
  {
    var objFromDB = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.Villa_Number);

    if (objFromDB is not null)
    {
      _context.VillaNumbers.Remove(objFromDB);
      _context.SaveChanges();

      TempData["success"] = "delete success!";

      return RedirectToAction("Index");
    }

    TempData["error"] = "delete failed!";
    return View();
  }
}