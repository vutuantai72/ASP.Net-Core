using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBaby.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.ViewComponents
{
    public class HeaderViewComponent: ViewComponent
    {
        private readonly ApplicationDbContext _context;
        public HeaderViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.user =  HttpContext.Session.GetString("Name");
            return View();
        }
    }
}
