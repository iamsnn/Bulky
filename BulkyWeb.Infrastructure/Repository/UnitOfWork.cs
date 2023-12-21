using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Domain.Entities;
using BulkyWeb.Infrastructure.Data;

namespace BulkyWeb.Infrastructure.Repository
{
  public class UnitOfWork:IUnitOfWork
  {
    private readonly ApplicationDbContext _db;
    public IVillaRepository Villa { get; }
    public IVillaNumberRepository VillaNumber { get; }
    public UnitOfWork(ApplicationDbContext db)
    {
      _db = db;
      Villa = new VillaRepository(_db);
      VillaNumber = new VillaNumberRepository(_db);
    }

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}
