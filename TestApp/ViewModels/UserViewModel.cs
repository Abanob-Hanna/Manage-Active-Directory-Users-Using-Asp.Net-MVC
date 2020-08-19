using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Web;

namespace ActiveDirectoryUserManagerWithMVC.ViewModels
{
    public class UserViewModel
    {

        public int Id { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }
        public string Samaccountname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserLogonName { get; set; }
        public string UserPassword { get; set; }
        public string UserNewPassword { get; set; }
        public string UserDomain { get; set; }
        public List<String> Domains { get; set; }
    }
}