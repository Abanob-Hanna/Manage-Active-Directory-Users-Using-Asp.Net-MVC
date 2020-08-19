using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ActiveDirectoryUserManagerWithMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string Samaccountname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Display(Name = "User Logon Name")]
        public string UserLogonName { get; set; }
        public string UserPassword { get; set; }
        //public List<string> Domains { get; set; }
    }
}