using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBaby.Data.Model
{
    [Table("FileImages")]
    public class FileImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ImageID { get; set; }

        public int ProductID { get; set; }

        public string Images { get; set; }

        [ForeignKey("ProductID")]
        public virtual Product Product { get; set; }
    }
}
