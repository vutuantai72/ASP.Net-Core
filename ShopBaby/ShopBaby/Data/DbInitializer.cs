using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.DependencyInjection;
using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            //context.Database.EnsureCreated();

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
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var user = new ApplicationUser { UserName = "vutuantai72@gmail.com", Email = "vutuantai72@gmail.com" };
            var result = await userManager.CreateAsync(user, "Tai123@");


            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var users = new IdentityRole("Admin");
                var res = await roleManager.CreateAsync(users);
                if (res.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }

            if (context.Contacts.Any())
            {
                return;
            }
            else {
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
}
