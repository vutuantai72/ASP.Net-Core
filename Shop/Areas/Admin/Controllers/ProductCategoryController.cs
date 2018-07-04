using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Model;

namespace Shop.Areas.Admin.Controllers
{
    [Route("Admin")]
    [Area("Admin")]
    public class ProductCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProductCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        protected void Save() => _context.SaveChanges();

        
        [Route("ProductCategory")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCategories.ToListAsync());
        }

        [Route("ProductCategory/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        [Route("ProductCategory/Create")]
        public IActionResult Create(ProductCategory productCategory)
        {
            if (!ModelState.IsValid) return View(productCategory);
            _context.Add(productCategory);
            Save();
            return RedirectToAction("Index");
        }

        [Route("ProductCategory/Delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _context.ProductCategories.Find(id);
            _context.ProductCategories.Remove(data);
            Save();
            return RedirectToAction("Index");
        }

        [Route("ProductCategory/Update/{id}")]
        public IActionResult Update(int id)
        {
            var productCategory = _context.ProductCategories.Find(id);
            return View(productCategory);
        }

        [Route("ProductCategory/Update/{id}")]
        [HttpPost]
        public IActionResult Update(ProductCategory productCategory)
        {
            _context.ProductCategories.Update(productCategory);
            Save();
            return RedirectToAction("Index");
        }
    }
}