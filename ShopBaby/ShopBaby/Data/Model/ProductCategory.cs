using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data.Model
{
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Chỉ ra ID kiểu int sẽ tự tăng 
        public int ID { set; get; }

        [Required]
        [MaxLength(256)]
        public string Name { set; get; }

        [MaxLength(256)]
        public string Alias { set; get; }

        [MaxLength(500)]
        public string Description { set; get; }
        public int? ParentID { set; get; }
        public int? DisplayOrder { set; get; }

        [MaxLength(256)]
        public string Image { set; get; }

        public bool? HomeFlag { set; get; }
        public DateTime? CreatedDate { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }

        [MaxLength(256)]
        public string MetaKeyword { set; get; }

        [MaxLength(256)]
        public string MetaDescription { set; get; }

        public bool Status { set; get; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
    }
}
