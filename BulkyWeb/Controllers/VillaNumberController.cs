using BulkyWeb.Infrastructure.Repository;
using BulkyWeb.ModelViews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Controllers;

public class VillaNumberController : Controller
{
  private readonly UnitOfWork _unitOfWork;

  public VillaNumberController(UnitOfWork _unitOfWork)
  {
    this._unitOfWork = _unitOfWork;
  }

  public IActionResult Index()
  {
    // load the navigation property and then display it
    var villaNumbers = _unitOfWork.VillaNumber.GetAll(includeProperties:"Villa");
    return View(villaNumbers);
  }

  public IActionResult Create()
  {
    VillaNumberVM vm = new()
    {
      VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
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
    bool numberExists = _unitOfWork.VillaNumber.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

    if (ModelState.IsValid && !numberExists)
    {
      _unitOfWork.VillaNumber.Add(obj.VillaNumber);
      _unitOfWork.Save();

      TempData["success"] = "create success!";

      return RedirectToAction(nameof(Index));
    }

    if (numberExists)
    {
      TempData["error"] = "create failed!";
    }

    obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
    {
      Text = u.Name,
      Value = u.Id.ToString()
    });
    
    return View(obj);
  }

  public IActionResult Update(int villaNumberId)
  {
    VillaNumberVM villaNumberVm = new VillaNumberVM
    {
      VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId),
      VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
      {
        Text = u.Name,
        Value = u.Id.ToString()
      })
    }; 

    if (villaNumberVm.VillaNumber == null) return RedirectToAction("Error", "Home");

    return View(villaNumberVm);
  }

  [HttpPost]
  public IActionResult Update(VillaNumberVM obj)
  {
    if (ModelState.IsValid)
    {
      _unitOfWork.VillaNumber.Update(obj.VillaNumber);
      _unitOfWork.Save();

      TempData["success"] = "Update success!";

      return RedirectToAction(nameof(Index));
    }

    
    TempData["error"] = "Update failed!";
    

    obj.VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
    {
      Text = u.Name,
      Value = u.Id.ToString()
    });

    return View(obj);
  }

  public IActionResult Delete(int villaNumberId)
  {
    VillaNumberVM villaNumberVm = new VillaNumberVM
    {
      VillaNumber = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId),
      VillaList = _unitOfWork.Villa.GetAll().Select(u => new SelectListItem
      {
        Text = u.Name,
        Value = u.Id.ToString()
      })
    };

    var obj = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == villaNumberId);

    if (obj == null) return RedirectToAction("Error", "Home");

    return View(villaNumberVm);
  }

  [HttpPost]
  public IActionResult Delete(VillaNumberVM obj)
  {
    var objFromDB = _unitOfWork.VillaNumber.Get(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

    if (objFromDB is not null)
    {
      _unitOfWork.VillaNumber.Remove(objFromDB);
      _unitOfWork.Save();

      TempData["success"] = "delete success!";

      return RedirectToAction(nameof(Index));
    }

    TempData["error"] = "delete failed!";
    return View();
  }
}