using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using BabyInABagServer.Models.VMs;

namespace BabyInABagServer.Models.Repos
{
    public class LoginRepository
    {
        public string LoginAuthenticate(CustomerAdminLogin cal)
        {
            CustomerAdminLogin calAuth = cal;

            Authenticator auth = new Authenticator();

            string username = calAuth.EnteredUsername;
            string password = calAuth.EnteredPassword;

            bool verified = auth.CheckHash(username, password);

            if (verified)
            {
                bool isAdmin = auth.IsAdminCheck(username);
                if (isAdmin == true)
                    return "Admin Login Success";
                else
                    return "Login Success";
            }
            else
                return "Username or Password is incorrect!";

            //return auth.generateHash(username, password);
        }
    }
}