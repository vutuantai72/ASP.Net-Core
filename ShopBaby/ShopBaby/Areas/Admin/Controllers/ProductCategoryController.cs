using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data;
using ShopBaby.Data.Model;

namespace ShopBaby.Areas.Admin.Controllers
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

            #region Alias
            string str = productCategory.Name.ToLower();
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string input = str.Normalize(NormalizationForm.FormD);
            str = Regex.Replace(input, @"\s", "-", RegexOptions.Compiled);
            str = regex.Replace(str, string.Empty).Replace(Convert.ToChar(273), 'd').Replace(Convert.ToChar(272), 'D');
            str = Regex.Replace(str, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
            str = str.Trim('-', '_');
            str = Regex.Replace(str, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            productCategory.Alias = str;
            #endregion

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