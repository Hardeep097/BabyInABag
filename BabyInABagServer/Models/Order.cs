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
        public int Order_Id { get; set; }
        public System.DateTime Order_Date_Placed { get; set; }
        public string Order_Status { get; set; }
        public string Order_Details { get; set; }
        public System.DateTime Order_Date_Paid { get; set; }

        [ForeignKey("Customer")]
        public int Customer_Id { get; set; }

        public string Shipping_Address { get; set; }
        public string Invoice_Status { get; set; }


        public virtual Customer Customer { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}