using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using BabyInABagServer.Models.VMs;
using BabyInABagServer.Models;
using System.Diagnostics;
using BabyInABagServer.Services;

namespace BabyInABagServer.Models.VMs
{
    public class Authenticator
    {
        private Context db = new Context();
        private string hash = "";

        public string GenerateHash(string username, string password)
        {
            string saltAndPwd = String.Concat(username, password);
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256Hash = new SHA256Managed();
            byte[] hashedData = sha256Hash.ComputeHash(encoder.GetBytes(saltAndPwd));
            string hashedPwd = String.Concat(ByteArrayToString(hashedData));
            return hashedPwd;
        }

        public string ByteArrayToString(byte[] input)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < input.Length; i++)
            {
                output.Append(input[i].ToString("X2"));
            }
            return output.ToString();
        }

        public bool CheckHash(string username, string plaintextPassword)
        {
            string[] usernameSplit = SplitUsernameArray(username);

            string plainTextHashed = "";
            //test
            try
            {
                bool admin = IsAdminCheck(username);

                if (admin == true)
                {
                    plainTextHashed = GenerateHash(usernameSplit[1], plaintextPassword);
                    return CompareHash(plainTextHashed);
                }
                else
                {
                    List<Customer> customers = new List<Customer>();
                    customers = db.Customers.ToList();
                    plainTextHashed = GenerateHash(username, plaintextPassword);
                    for (int c = 0; c < customers.Count; c++)
                    {
                        if (customers[c].Username.Equals(username))
                        {
                            hash = customers[c].Password;
                        }
                    }
                }
            }
            catch (Exception)
            {
                hash = "";
            }

            return CompareHash(plainTextHashed);
        }

        public string[] SplitUsernameArray(string username)
        {
            string[] seperator = { "." };
            string[] usernameArray = username.Split(seperator, StringSplitOptions.RemoveEmptyEntries);
            return usernameArray;
        }

        public bool CompareHash(string plainTextHashed)
        {
            if (plainTextHashed.Equals(hash))
                return true;
            else 
                return false;
        }

        public bool IsAdminCheck(string username)
        {
            string[] usernameArray = SplitUsernameArray(username);
            if (usernameArray.Length == 2)
            {
                if (usernameArray[0].Equals("bibs"))
                {
                    List<Admin> admins = new List<Admin>();
                    admins = db.Admins.ToList();
                    for (int a = 0; a < admins.Count; a++)
                    {
                        if (admins[a].Username.Equals(usernameArray[1]))
                        {
                            hash = admins[a].Password;
                            return true;
                        }
                    }
                }
            }
            return false;
        }
    }
}