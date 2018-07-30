using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data;
using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShopBaby.Areas.Admin.Controllers
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
        public async Task<IActionResult> Create()
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            categories = _context.ProductCategories.Where(p => p.ParentID != null).ToList();
            ViewBag.list = categories;

            return View();
        }

        [HttpPost]
        [Route("Product/Create")]
        public IActionResult Create(Product product, IFormFile singleFile)
        {
            List<ProductCategory> categories = new List<ProductCategory>();
            categories = _context.ProductCategories.ToList();
            ViewBag.list = categories;

            if (!ModelState.IsValid) return View(product);//Kiểm tra có hợp lệ hay k nếu k hợp lệ trả về view cũ

            if (product.HotFlag == null)
            {
                product.HotFlag = false;
            }

            if (product.HomeFlag == null)
            {
                product.HomeFlag = false;
            }

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

            #endregion Alias

            #region singleFile

            var fileName = string.Empty;
            var newFileNames = string.Empty;
            if (singleFile != null)
            {
                if (singleFile.Length > 0)
                {
                    //Lấy tên cho file
                    fileName = ContentDispositionHeaderValue.Parse(singleFile.ContentDisposition).FileName.Trim('"');

                    //Tạo tên duy nhất
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Lấy đuôi file (.jpg)
                    var FileExtension = Path.GetExtension(fileName);

                    //Kết nối  myUniqueFileName + FileExtension
                    newFileNames = myUniqueFileName + FileExtension;

                    //Lưu file
                    fileName = Path.Combine(_appEnvironment.WebRootPath, "images/product/") + $@"\{newFileNames}";

                    //Kết nối biến vào database
                    product.Image = "images/product/" + newFileNames;

                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        singleFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }

            #endregion singleFile

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
        public IActionResult Update(Product product, IFormFile singleFile)
        {
            if (!ModelState.IsValid) return View(product);
            List<ProductCategory> categories = new List<ProductCategory>();
            categories = _context.ProductCategories.ToList();
            ViewBag.list = categories;

            if (product.HotFlag == null)
            {
                product.HotFlag = false;
            }

            if (product.HomeFlag == null)
            {
                product.HomeFlag = false;
            }

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

            #endregion Alias

            #region Update singleImage

            var fileName = string.Empty;
            var files = HttpContext.Request.Form.Files;
            if (files.Count != 0)
            {
                if (singleFile != null)
                {
                    if (singleFile.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(singleFile.ContentDisposition).FileName.Trim('"');
                        var fileNewName = Path.Combine(_appEnvironment.WebRootPath, "images/product/") + $@"\{fileName}";

                        //Kết nối biến vào database
                        product.Image = "images/product/" + fileName;

                        using (FileStream fs = System.IO.File.Create(fileNewName))
                        {
                            singleFile.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                }
            }

            #endregion Update singleImage

            _context.Products.Update(product);
            Save();
            return RedirectToAction("Index");
        }

        [Route("Product/MoreImages/{id}")]
        [HttpGet]
        public IActionResult MoreImages(int id)
        {
            List<FileImage> item = new List<FileImage>();
            item = _context.FileImages.Where(p => p.ProductID == id).ToList();
            ViewBag.images = item;
            return View();
        }

        [Route("Product/MoreImages/{id}")]
        [HttpPost]
        public IActionResult MoreImages(FileImage image, IFormFile singleFile, Product product)
        {
            image.ProductID = product.ID;

            #region singleFile

            var fileName = string.Empty;
            var newFileNames = string.Empty;
            if (singleFile != null)
            {
                if (singleFile.Length > 0)
                {
                    //Lấy tên cho file
                    fileName = ContentDispositionHeaderValue.Parse(singleFile.ContentDisposition).FileName.Trim('"');

                    //Tạo tên duy nhất
                    var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                    //Lấy đuôi file (.jpg)
                    var FileExtension = Path.GetExtension(fileName);

                    //Kết nối  myUniqueFileName + FileExtension
                    newFileNames = myUniqueFileName + FileExtension;

                    //Lưu file
                    fileName = Path.Combine(_appEnvironment.WebRootPath, "images/product/") + $@"\{newFileNames}";

                    //Kết nối biến vào database
                    image.Images = "images/product/" + newFileNames;

                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                        singleFile.CopyTo(fs);
                        fs.Flush();
                    }
                }
            }

            _context.Add(image);
            Save();

            #endregion singleFile

            return RedirectToAction("MoreImages");
        }

        [Route("Product/DeleteImages/{id}")]
        [HttpGet]
        public IActionResult DeleteImages(int id)
        {
            var item = _context.FileImages.Find(id).ProductID;
            var data = _context.FileImages.Find(id);
            _context.FileImages.Remove(data);
            Save();
            return RedirectToAction("MoreImages", new { id = item });
        }
    }
}