using BabyInABagServer.Models.VMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models.Repos
{
    public class CustomerRepository
    {
        public Customer GetCustomer (CustomerAdminSignup cas)
        {
            Customer customer = new Customer();
            customer.First_Name = cas.FirstName;
            customer.Last_Name = cas.LastName;
            customer.Username = cas.Username;
            customer.Password = cas.Password;
            customer.Customer_Email = cas.Email;
            customer.Customer_Phone = cas.Phone;
            return customer;
        }
    }
}