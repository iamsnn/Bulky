using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers;

public class VillaController : Controller
{
  private readonly IUnitOfWork _unitOfWork;

  public VillaController(IUnitOfWork unitOfWork)
  {
    _unitOfWork = unitOfWork;
  }

  public IActionResult Index()
  {
    var villas = _unitOfWork.Villa.GetAll();
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
      _unitOfWork.Villa.Add(obj);
      _unitOfWork.Save();

      TempData["success"] = "create success!";

      return RedirectToAction("Index", "Villa");
    }
    TempData["error"] = "create failed!";
    return View();
  }

  public IActionResult Update(int villaId)
  {
    var obj = _unitOfWork.Villa.Get(x => x.Id == villaId);

    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Update(Villa obj)
  {
    if (ModelState.IsValid && obj.Id > 0)
    {
      _unitOfWork.Villa.Update(obj);
      _unitOfWork.Save();

      TempData["success"] = "update success!";

      return RedirectToAction("Index", "Villa");
    }

    TempData["error"] = "update failed!";
    return View();
  }

  public IActionResult Delete(int villaId)
  {
    var obj = _unitOfWork.Villa.Get(x => x.Id == villaId);


    if (obj == null) return RedirectToAction("Error", "Home");

    return View(obj);
  }

  [HttpPost]
  public IActionResult Delete(Villa obj)
  {
    var objFromDB = _unitOfWork.Villa.Get(x => x.Id == obj.Id);

    if (objFromDB is not null)
    {
      _unitOfWork.Villa.Remove(objFromDB);
      _unitOfWork.Save();

      TempData["success"] = "delete success!";

      return RedirectToAction("Index", "Villa");
    }

    TempData["error"] = "delete failed!";
    return View();
  }
}