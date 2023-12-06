using BulkyWeb.Domain.Entities;
using BulkyWeb.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class VillaController : Controller
{
  private readonly ApplicationDbContext _context;

  public VillaController(ApplicationDbContext context)
  {
    _context = context;
  }

  public IActionResult Index()
  {
    var villas = _context.Villas.ToList();
    return View(villas);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create(Villa obj)
  {
    if (obj.Name == obj.Description)
    {
      ModelState.AddModelError("name", "same");
    }


    if (ModelState.IsValid)
    {
      _context.Villas.Add(obj);
      _context.SaveChanges();

      return RedirectToAction("Index", "Villa");
    }

    return View();
  }

  public IActionResult Update(int villaId)
  {
    Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);


    if (obj == null)
    {
      return RedirectToAction("Error", "Home");
    }

    return View(obj);
  }

  [HttpPost]
  public IActionResult Update(Villa obj)
  {
    if (ModelState.IsValid && obj.Id > 0)
    {
      _context.Villas.Update(obj);
      _context.SaveChanges();

      return RedirectToAction("Index", "Villa");
    }

    return View();
  }

  public IActionResult Delete(int villaId)
  {
    Villa? obj = _context.Villas.FirstOrDefault(x => x.Id == villaId);


    if (obj == null)
    {
      return RedirectToAction("Error", "Home");
    }

    return View(obj);
  }

  [HttpPost]
  public IActionResult Delete(Villa obj)
  {
    Villa? objFromDB = _context.Villas.FirstOrDefault(x => x.Id == obj.Id);

    if (objFromDB is not null)
    {
      _context.Villas.Remove(objFromDB);
      _context.SaveChanges();

      return RedirectToAction("Index", "Villa");
    }

    return View();
  }
}