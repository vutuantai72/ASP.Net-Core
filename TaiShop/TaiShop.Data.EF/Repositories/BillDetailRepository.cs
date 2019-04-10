using System;
using System.Collections.Generic;
using System.Text;
using TaiShop.Data.Entities;
using TaiShop.Data.IRepositories;

namespace TaiShop.Data.EF.Repositories
{
    public class BillDetailRepository : EFRepository<BillDetail, int>, IBillDetailRepository
    {
        public BillDetailRepository(AppDbContext context) : base(context)
        {
        }
    }
}
