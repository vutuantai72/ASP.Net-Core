using System;
using System.Collections.Generic;
using System.Text;
using TaiShop.Data.Entities;
using TaiShop.Data.IRepositories;

namespace TaiShop.Data.EF.Repositories
{
    public class SizeRepository : EFRepository<Size, int>, ISizeRepository
    {
        public SizeRepository(AppDbContext context) : base(context)
        {
        }
    }
}
