﻿using BulkyWeb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyWeb.Application.Common.Interfaces
{
  public interface IVillaNumberRepository : IRepository<VillaNumber>
  {
    void Update(VillaNumber entity);
  }
}
