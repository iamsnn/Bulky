using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyWeb.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BulkyWeb.Infrastructure.Data
{
  public class ApplicationDbContext:DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    // automatically generate
    public DbSet<Villa> Villas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      //base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<Villa>().HasData(new Villa
      {
        Id = 1, 
        Name = "a",
        Description = "des",
        ImageUrl = "https://placehold.co/600*402",
        Occupancy = 2,
        Price = 200,
        Sqft = 500
      });
    }
  }
}
