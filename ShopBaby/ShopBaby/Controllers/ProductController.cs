using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data;

namespace ShopBaby.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int id)
        {
            var products = await _context.Products.Where(p => p.CategoryID == id).ToListAsync();
            
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var detail = await _context.Products.Where(p => p.ID == id).ToListAsync();
            return View(detail.SingleOrDefault());
        }
    }
}