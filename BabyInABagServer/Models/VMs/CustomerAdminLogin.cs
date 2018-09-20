using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BabyInABagServer.Models;
using BabyInABagServer.Services;

namespace BabyInABagServer.Models.VMs
{
    public class CustomerAdminLogin
    {
        private Context db = new Context();
        public Customer Customer { get; set; }
        public Admin Admin { get; set; }

        [Required]
        [DisplayName("Username")]
        public string EnteredUsername { get; set; }

        [Required]
        [DisplayName("Password")]
        public string EnteredPassword { get; set; }
    }
}