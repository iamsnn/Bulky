using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class VillaController : Controller
{
  private readonly IVillaRepository _villaRepo;

  public VillaController(IVillaRepository villaRepo)
  {
    _villaRepo = villaRepo;
  }

  public IActionResult Index()
  {
    var villas = _villaRepo.GetAll();
    return View(villas);
  }

  public IActionResult Create()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Create(Villa obj)
  {
    if (obj.Name == obj.Description) ModelState.AddModelError("name", "same");


    if (ModelState.IsValid)
    {
      _villaRepo.Add(obj);
      _villaRepo.Save();

      TempData["success"] = "create success!";

      return RedirectToAction("Index", "Villa");
    }
    TempData["error"] = "create failed!";
    return View();
  }

  public IActionResult Update(int villaId)
  {
    var obj = _villaRepo.Get(x => x.Id == villaId);

    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Update(Villa obj)
  {
    if (ModelState.IsValid && obj.Id > 0)
    {
      _villaRepo.Update(obj);
      _villaRepo.Save();

      TempData["success"] = "update success!";

      return RedirectToAction("Index", "Villa");
    }

    TempData["error"] = "update failed!";
    return View();
  }

  public IActionResult Delete(int villaId)
  {
    var obj = _villaRepo.Get(x => x.Id == villaId);


    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Delete(Villa obj)
  {
    var objFromDB = _villaRepo.Get(x => x.Id == obj.Id);

    if (objFromDB is not null)
    {
      _villaRepo.Remove(objFromDB);
      _villaRepo.Save();

      TempData["success"] = "delete success!";

      return RedirectToAction("Index", "Villa");
    }

    TempData["error"] = "delete failed!";
    return View();
  }
}