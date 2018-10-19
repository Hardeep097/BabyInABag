using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BabyInABagServer.Models;

namespace BabyInABagServer.Models.VMs
{
    public class CustomerOrder
    {
        public Order Order { get; set; }
        public List<CustomerProduct> CustomerProducts { get; set; }

        public CustomerOrder()
        {
            CustomerProducts = new List<CustomerProduct>();
        }
    }

    public class CustomerProduct
    {
        public int Id { get; set; }
        public string ProductInfo { get; set; }
        public bool isAdded { get; set; }
    }
}