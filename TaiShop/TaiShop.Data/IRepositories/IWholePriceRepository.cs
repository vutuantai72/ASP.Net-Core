﻿using System;
using System.Collections.Generic;
using System.Text;
using TaiShop.Data.Entities;
using TaiShop.Infrastructure.Interfaces;

namespace TaiShop.Data.IRepositories
{
    public interface IWholePriceRepository : IRepository<WholePrice, int>
    {
    }
}
