using Microsoft.Azure.KeyVault.Models;
using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            //if (context.ProductCategories.Any())
            //{
            //    return;
            //}

            //var productCategories = new ProductCategory[]
            //    {
            //        new ProductCategory{Name="Máy Lạnh",Alias="may-lanh" },
            //        new ProductCategory{Name="Ti Vi",Alias="ti-vi" },
            //        new ProductCategory{Name="Gia Dụng",Alias="gia-dung" },
            //    };
            //foreach (ProductCategory pc in productCategories)
            //{
            //    context.ProductCategories.Add(pc);
            //}

            if (context.Contacts.Any())
            {
                return;
            }

            var contacts = new Model.Contact
            {
                Address = "247/17A Lạc Long Quân P.3 Q.11",
                Email = "vutuantai72@gmail.com",
                Phone = "0938154066"
            };
            context.Contacts.Add(contacts);

            context.SaveChanges();
        }
    }
}
