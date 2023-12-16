using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Domain.Entities;
using BulkyWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Infrastructure.Repository
{
  public class VillaRepository : IVillaRepository
  {
    private readonly ApplicationDbContext _db;

    public VillaRepository(ApplicationDbContext db) => _db = db;

    public IEnumerable<Villa> GetAll(Expression<Func<Villa, bool>>? filter = null, string? includeProperties = null)
    {
      IQueryable<Villa> query = _db.Set<Villa>();
      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (!string.IsNullOrEmpty(includeProperties))
      {
        // Case Sensitive: Villa, VillaNumber
        foreach (var includeProperty in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProperty);
        }
      }

      return query.ToList();
    }

    public Villa Get(Expression<Func<Villa, bool>> filter, string? includeProperties = null)
    {
      IQueryable<Villa> query = _db.Set<Villa>();
      query = query.Where(filter);

      if (!string.IsNullOrEmpty(includeProperties))
      {
        // Case Sensitive: Villa, VillaNumber
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProperty);
        }
      }

      return query.FirstOrDefault();
    }

    public void Add(Villa entity)
    {
      _db.Add(entity);
    }

    public void Update(Villa entity)
    {
      _db.Update(entity);
    }

    public void Remove(Villa entity)
    {
      _db.Remove(entity);
    }

    public void Save()
    {
      _db.SaveChanges();
    }
  }
}
