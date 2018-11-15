using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBaby.Data;
using ShopBaby.Data.Model;
using ShopBaby.Helpers;
using ShopBaby.Models;

namespace ShopBaby.Controllers
{
    [Route("Cart")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public CartController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<OrderDetail> list = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            if (list != null)
            {
                iTongSoLuong = list.Sum(p => p.Quantity);
            }
            return iTongSoLuong;
        }

        [Route("ListCartsEmpty")]
        public IActionResult Empty()
        {
            return View();
        }

        [Route("ListCarts")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Name") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            if (cart != null)
            {
                ViewBag.cart = cart;
                ViewBag.count = TongSoLuong();
                ViewBag.total = cart.Sum(item =>
                {
                    if (item.Product.PromotionPrice != null)
                    {
                        return (item.Product.PromotionPrice.Value * item.Quantity);
                    }
                    else
                    {
                        return (item.Product.Price * item.Quantity);
                    }
                });
                return View();
            }
            return RedirectToAction("Empty","Cart");
        }

        [Route("AddToCart/{id}")]
        public IActionResult AddToCart(int id)
        {
            if (SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart") == null)
            {
                List<OrderDetail> cart = new List<OrderDetail>();
                cart.Add(new OrderDetail { Product = _dbContext.Products.Find(id), Quantity = 1 });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<OrderDetail> cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
                int index = isExist(id);
                if (index != -1)
                {
                    cart[index].Quantity++;
                }
                else
                {
                    cart.Add(new OrderDetail { Product = _dbContext.Products.Find(id), Quantity = 1 });
                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("Index");
        }

        [Route("remove/{id}")]
        public IActionResult Remove(int id)
        {
            List<OrderDetail> cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            int index = isExist(id);
            cart.RemoveAt(index);
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");
        }

        private int isExist(int id)
        {
            List<OrderDetail> cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.ID.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }

        public IActionResult Checkout()
        {
            var cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.count = TongSoLuong();
            ViewBag.total = cart.Sum(item =>
            {
                if (item.Product.PromotionPrice != null)
                {
                    return (item.Product.PromotionPrice * item.Quantity);
                }
                return (item.Product.Price * item.Quantity);
            });
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            List<OrderDetail> cart = SessionHelper.GetObjectFromJson<List<OrderDetail>>(HttpContext.Session, "cart");
            order.CustomerId = HttpContext.Session.GetInt32("ID").Value;
            order.CreatedDate = DateTime.Now;
            order.Status = false;
            _dbContext.Add(order);
            _dbContext.SaveChanges();

            var customer = _dbContext.Customers.SingleOrDefault(p => p.FullName == HttpContext.Session.GetString("Name"));
            customer.OrderID++;
            foreach (var item in cart)
            {
                OrderDetail orderDetail = new OrderDetail();
                orderDetail.Quantity = item.Quantity;
                orderDetail.OrderID = order.ID;
                if (item.Product.PromotionPrice != null)
                {
                    orderDetail.Price = item.Product.PromotionPrice.Value;
                }
                else
                {
                    orderDetail.Price = item.Product.Price;
                }
                orderDetail.ProductID = item.Product.ID;
                _dbContext.Add(orderDetail);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}