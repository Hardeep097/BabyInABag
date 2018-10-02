using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models
{
    public class ProductCategory
    {
        [Key]
        public int Product_Category_Id { get; set; }

        public string Product_Category { get; set; }


        [NotMapped]
        public List<ProductCategory> CategoryCollection { get; set; }

    }
}