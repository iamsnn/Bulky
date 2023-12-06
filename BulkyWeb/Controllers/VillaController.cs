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
}