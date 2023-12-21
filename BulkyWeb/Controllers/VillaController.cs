using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BulkyWeb.Controllers;

public class VillaController : Controller
{
  private readonly IUnitOfWork _unitOfWork;
  private readonly IWebHostEnvironment _webHostEnvironment;

  public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
  {
    _unitOfWork = unitOfWork;
    _webHostEnvironment = webHostEnvironment;
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
      if (obj.Image != null)
      {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
        string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImage");

        using var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create);
        obj.Image.CopyTo(fileStream);

        obj.ImageUrl = @"\images\VillaImage\" + fileName;
      }
      else
      {
        obj.ImageUrl = "https://placehold.co/600*400";
      }


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