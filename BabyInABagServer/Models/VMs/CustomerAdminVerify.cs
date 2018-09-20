using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models.VMs
{
    public class CustomerAdminVerify
    {
        [DisplayName("Enter your 4-Digit Verification Code:")]
        public string Verify { get; set; }
    }
}