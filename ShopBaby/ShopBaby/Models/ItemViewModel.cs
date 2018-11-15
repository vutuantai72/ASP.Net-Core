using ShopBaby.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Models
{
    public class ItemViewModel
    {
        public Product Product { get; set; }

        public int Quantity { get; set; }
    }
}
