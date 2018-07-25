using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using ShopBaby.Data;
using ShopBaby.Data.Model;
using ShopBaby.Models;

namespace ShopBaby.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> GetCategory(int id)
        {
            var list = await _context.ProductCategories.Where(x => x.ID == id).ToListAsync();
            return View(list);
        }

        public async Task<IActionResult> _Header()
        {
            var contact = await _context.Contacts.ToListAsync();
            return View(contact);
        }

    }
}