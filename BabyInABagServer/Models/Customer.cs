using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models
{
    public class Customer
    {
        [Key]
        public int Customer_Id { get; set; }
        [Display(Name = "First Name")]
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        [Display(Name = "Last Name")]
        public string Last_Name { get; set; }
        public string Customer_Phone { get; set; }
        public string Customer_Email { get; set; }
        public string Customer_Details { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}