using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveDirectoryUserManagerWithMVC.Models;

namespace ActiveDirectoryUserManagerWithMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

   
        

        //public ActionResult GetAllGroups()
        //{
        //    List<Group> ADGroups = GetallGroups();
        //    return View(ADGroups);
        //}

        public static List<Group> GetallGroups()
        {
            List<Group> AdGroups = new List<Group>();
            var ctx = new PrincipalContext(ContextType.Domain, "MSAH", "OU=Dev OU,DC=MSAH,DC=com");
            GroupPrincipal _groupPrincipal = new GroupPrincipal(ctx);
            PrincipalSearcher srch = new PrincipalSearcher(_groupPrincipal);
            var result = srch.FindAll();
            foreach(Principal G in result)
            {
                AdGroups.Add(new Group { GroupName = G.ToString() });
            }
            return AdGroups;
        }
    }
}