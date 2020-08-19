using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveDirectoryUserManagerWithMVC.Models;

namespace ActiveDirectoryUserManagerWithMVC.Controllers
{
    public class SearchController : Controller
    {
        //MSAH.com My Domain Controller which i created 
        //OU= Dev OU --Organizational Unit which i created 
        //and create users and groups inside it
        PrincipalContext context = new PrincipalContext(ContextType.Domain, "MSAH", "OU=Dev OU,DC=MSAH,DC=com");


        // GET: Search
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string searchString)
        {
            List<User> AdUsers = new List<User>();
            if (!string.IsNullOrWhiteSpace(searchString))
            {
                searchString = searchString.ToLower();
                UserPrincipal _userPrincipal = new UserPrincipal(context);
                _userPrincipal.Name = "*";
                var searcher = new PrincipalSearcher();
                searcher.QueryFilter = _userPrincipal;
                var results = searcher.FindAll();
                foreach (Principal p in results)
                {
                    AdUsers.Add(new User
                    {
                        DisplayName = p.DisplayName,
                        Samaccountname = p.SamAccountName,
                        UserName = p.UserPrincipalName,
                        UserLogonName = p.SamAccountName
                    });
                }
            }

            return PartialView("_Search", AdUsers.Where(a => a.DisplayName.ToLower().Contains(searchString)).ToList());
        }
    }
}