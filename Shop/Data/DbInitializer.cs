using Shop.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ProductCategories.Any())
            {
                return;
            }

            var productCategories = new ProductCategory[]
                {
                    new ProductCategory{Name="Máy Lạnh",Alias="may-lanh" },
                    new ProductCategory{Name="Ti Vi",Alias="ti-vi" },
                    new ProductCategory{Name="Gia Dụng",Alias="gia-dung" },
                };
            foreach (ProductCategory pc in productCategories)
            {
                context.ProductCategories.Add(pc);
            }
            context.SaveChanges();
        }
    }
}
