using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopBaby.Models;
using ShopBaby.Data;
using ShopBaby.Data.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShopBaby.Helpers;

namespace ShopBaby.Controllers
{

    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public AccountController(ApplicationDbContext dbContext)
        {                                                      
            _dbContext = dbContext;

        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("dang-nhap.html")]
        [HttpGet, AllowAnonymous] 
        public IActionResult Login(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("dang-nhap.html")]
        [HttpPost,AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel loginVm,string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            Customer customer = _dbContext.Customers.SingleOrDefault(p => p.Email == loginVm.UserName && 
            p.Password == loginVm.Password);
            if (customer == null)
            {
                ViewBag.ThongBaoLoi = "Sai thông tin đăng nhập";
                return View();
            }
            ////lấy thông tin user đang đăng nhập
            //var claims = new List<Claim> { new Claim(ClaimTypes.Name, customer.FullName) };
            ////tạo identity
            //ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
            //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
            //await HttpContext.SignInAsync(principal);

            //lưu session

            HttpContext.Session.SetInt32("ID", customer.ID);
            HttpContext.Session.SetString("Name", customer.FullName);
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index","Home");
            }

        }


        [Route("dang-ky.html")]
        [HttpGet,AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [Route("dang-ky.html")]
        [HttpPost, AllowAnonymous]
        public IActionResult Register(RegisterViewModel registerVm, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Customer { FullName = registerVm.FullName, Email = registerVm.Email,Password = registerVm.Password };
                _dbContext.Add(user);
                _dbContext.SaveChanges();
                return RedirectToAction("Login", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(registerVm);
        }
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.Remove("Name");//xoá session
            return RedirectToAction("Index","Home");
        }

    }
}