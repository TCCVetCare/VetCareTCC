using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VetCareTCC.Models
{
    public class UserModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public UserRole role { get; set; }


        public enum UserRole
        {
            Admin,
            Customer
        }
    }
}