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
        int PageSize = 1;//thay đổi hoặc lưu appsetting
        public async Task<IActionResult> Index(int id,int page = 1)
        {

            var products = await _context.Products.Where(p => p.CategoryID == id).ToListAsync();

            ViewBag.ID = id;
            ViewBag.TrangHienTai = page;
            ViewBag.TongSoTrang = Math.Ceiling(products.Count * 1.0 / PageSize);
            products = products.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            
            return View(products);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var detail = await _context.Products.Where(p => p.ID == id).ToListAsync();
            return View(detail.SingleOrDefault());
        }
    }
}