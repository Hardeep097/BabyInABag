using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BabyInABagServer.Models
{
    public class Admin
    {
        //Admin Model
        [Key]
        public int Admin_Id { get; set; }
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int Order_AuditOrder_Audit_Id { get; set; }
        public string Salt { get; set; }
    }
}