﻿using System.Linq.Expressions;
using BulkyWeb.Application.Common.Interfaces;
using BulkyWeb.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Infrastructure.Repository
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private readonly ApplicationDbContext _db;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext db) {
      _db = db;
      dbSet = _db.Set<T>();
    }

    public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
      IQueryable<T> query = dbSet;
      if (filter != null)
      {
        query = query.Where(filter);
      }

      if (!string.IsNullOrEmpty(includeProperties))
      {
        // Case Sensitive: T, TNumber
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProperty);
        }
      }

      return query.ToList();
    }

    public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
    {
      IQueryable<T> query = dbSet;
      query = query.Where(filter);

      if (!string.IsNullOrEmpty(includeProperties))
      {
        // Case Sensitive: T, TNumber
        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
          query = query.Include(includeProperty);
        }
      }

      return query.FirstOrDefault();
    }

    public void Add(T entity)
    {
      dbSet.Add(entity);
    }

    public void Remove(T entity)
    {
      dbSet.Remove(entity);
    }
  }
}
