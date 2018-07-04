﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Model;

namespace Shop.Areas.Admin.Controllers
{
    [Route("Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        public ProductController(ApplicationDbContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        protected void Save() => _context.SaveChanges();


        [Route("Product")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        [Route("Product/Create")]
        [HttpGet]
        public IActionResult Create()
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            categories = _context.ProductCategories.ToList();
            //SelectList productlist = new SelectList(categories, "ID", "Name");
            ViewBag.list = categories;

            return View();
        }

        [HttpPost]
        [Route("Product/Create")]
        public IActionResult Create(Product product)
        {

            #region Alias
            string str = product.Name.ToLower();
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string input = str.Normalize(NormalizationForm.FormD);
            str = Regex.Replace(input, @"\s", "-", RegexOptions.Compiled);
            str = regex.Replace(str, string.Empty).Replace(Convert.ToChar(273), 'd').Replace(Convert.ToChar(272), 'D');
            str = Regex.Replace(str, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
            str = str.Trim('-', '_');
            str = Regex.Replace(str, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            product.Alias = str;
            #endregion
            if (!ModelState.IsValid) return View(product);//Kiểm tra có hợp lệ hay k nếu k hợp lệ trả về view cũ

            #region up file image 
            var fileName = string.Empty;
            var files = HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                var newFileName = string.Empty;
                if (file.Length > 0)
                {
                    //Lấy tên cho file
                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    //Tạo tên duy nhất
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Lấy đuôi file (.jpg)
                    var FileExtension = Path.GetExtension(fileName);

                    //Kết nối  myUniqueFileName + FileExtension
                    newFileName = myUniqueFileName + FileExtension;

                    //Lưu file
                    fileName = Path.Combine(_appEnvironment.WebRootPath, "images/") + $@"\{newFileName}";

                    //Kết nối biến vào database
                    product.Image = "images/" + newFileName;

                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }
            #endregion

            _context.Add(product);
            Save();
            return RedirectToAction("Index");
        }

        [Route("Product/Delete/{id}")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var data = _context.Products.Find(id);
            _context.Products.Remove(data);
            Save();
            return RedirectToAction("Index");
        }

        [Route("Product/Update/{id}")]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Find(id);
            List<ProductCategory> categories = new List<ProductCategory>();
            categories = _context.ProductCategories.ToList();
            ViewBag.list = categories;
            return View(product);
        }

        [Route("Product/Update/{id}")]
        [HttpPost]
        public IActionResult Update(Product product)
        {
            if (!ModelState.IsValid) return View(product);
            var fileName = string.Empty;
            var files = HttpContext.Request.Form.Files;
            if (files.Count != 0)
            {
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var fileNewName = Path.Combine(_appEnvironment.WebRootPath, "images/") + $@"\{fileName}";

                        //Kết nối biến vào database
                        product.Image = "images/" + fileName;

                        using (FileStream fs = System.IO.File.Create(fileNewName))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            _context.Products.Update(product);
            Save();
            return RedirectToAction("Index");
        }
    }
}