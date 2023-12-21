using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BulkyWeb.Domain.Entities
{
  public class Villa
  {
    public int Id { get; set; }
    public required string Name { get; set; }
    [Display(Name = "Descriptions")]
    public string? Description { get; set; }
    [Range(10, 10000)]
    public double Price { get; set; }
    public int Sqft { get; set; }
    [Range(1, 10)]
    public int Occupancy { get; set; }
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile? Image { get; set; }
    public DateTime? CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
  }
}
