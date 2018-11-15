using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data;

namespace ShopBaby.Areas.Admin.Controllers
{
    [Route("Admin")]
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomerController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("Customer")]
        public async Task<IActionResult> Customer()
        {          
            return View(await _context.Orders.ToListAsync());
        }

        [Route("Bill")]
        public async Task<IActionResult> Bill(int id)
        {
            var list = await _context.OrderDetails.Where(p => p.OrderID == id)
                .Include(p => p.Product)
                .ToListAsync();
            return View(list);
        }

        [Route("Checkout")]
        public IActionResult Checkout(int id)
        {
            var check = _context.Orders.Find(id);
            check.Status = true;
            _context.SaveChanges();
            return RedirectToAction("Customer", "Customer");
        }
    }
}