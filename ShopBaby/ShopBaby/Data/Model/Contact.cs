using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data.Model
{
    [Table("Contacts")]
    public class Contact 
    {
        [Key]
        public int ID { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
