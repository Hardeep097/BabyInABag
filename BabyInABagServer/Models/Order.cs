using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "Order Number")]
        public int Order_Id { get; set; }
        [Display(Name = "Order Placed Date")]
        public System.DateTime Order_Date_Placed { get; set; }
        [Display(Name = "Order Status")]
        public string Order_Status { get; set; }
        [Display(Name = "Order Details")]
        public string Order_Details { get; set; }
        [Display(Name = "Order Paid Date")]
        public System.DateTime Order_Date_Paid { get; set; }
        [Display(Name = "Shipping Address")]
        public string Shipping_Address { get; set; }
        [Display(Name = "Invoice Status")]
        public string Invoice_Status { get; set; }

        [ForeignKey("Customer")]
        public int Customer_Id { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}