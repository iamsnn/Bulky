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
    bool numberExists = _context.VillaNumbers.Any(u => u.Villa_Number == obj.VillaNumber.Villa_Number);

    if (ModelState.IsValid && !numberExists)
    {
      _context.VillaNumbers.Add(obj.VillaNumber);
      _context.SaveChanges();

      TempData["success"] = "create success!";

      return RedirectToAction(nameof(Index));
    }

    if (numberExists)
    {
      TempData["error"] = "create failed!";
    }

    obj.VillaList = _context.Villas.ToList().Select(u => new SelectListItem
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
      VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId),
      VillaList = _context.Villas.ToList().Select(u => new SelectListItem
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
      _context.VillaNumbers.Update(obj.VillaNumber);
      _context.SaveChanges();

      TempData["success"] = "Update success!";

      return RedirectToAction(nameof(Index));
    }

    
    TempData["error"] = "Update failed!";
    

    obj.VillaList = _context.Villas.ToList().Select(u => new SelectListItem
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
      VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId),
      VillaList = _context.Villas.ToList().Select(u => new SelectListItem
      {
        Text = u.Name,
        Value = u.Id.ToString()
      })
    };

    var obj = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId);

    if (obj == null) return RedirectToAction("Error", "Home");

    return View(villaNumberVm);
  }

  [HttpPost]
  public IActionResult Delete(VillaNumberVM obj)
  {
    var objFromDB = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == obj.VillaNumber.Villa_Number);

    if (objFromDB is not null)
    {
      _context.VillaNumbers.Remove(objFromDB);
      _context.SaveChanges();

      TempData["success"] = "delete success!";

      return RedirectToAction(nameof(Index));
    }

    TempData["error"] = "delete failed!";
    return View();
  }
}