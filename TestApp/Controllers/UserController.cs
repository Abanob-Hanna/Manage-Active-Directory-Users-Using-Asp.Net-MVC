using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActiveDirectoryUserManagerWithMVC.Models;
using System.Net;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using ActiveDirectoryUserManagerWithMVC.ViewModels;

namespace ActiveDirectoryUserManagerWithMVC.Controllers
{
    public class UserController : Controller
    {
        //MSAH.com My Domain Controller which i created 
        //OU= Dev OU --Organizational Unit which i created 
        //and create users and groups inside it
        PrincipalContext context = new PrincipalContext(ContextType.Domain, "MSAH", "OU=Dev OU,DC=MSAH,DC=com");

        public static List<User> GetallAdUsers(PrincipalContext _context)
        {
            List<User> AdUsers = new List<User>();
            UserPrincipal _userPrincipal = new UserPrincipal(_context);
            _userPrincipal.Name = "*";
            var searcher = new PrincipalSearcher();
            searcher.QueryFilter = _userPrincipal;
            var results = searcher.FindAll();
            foreach (Principal p in results)
            {
                AdUsers.Add(new User
                {
                    DisplayName = p.DisplayName,
                    Samaccountname = p.SamAccountName
                });
            }
            return AdUsers;
        }


        public static List<string> EnumerateDomains()
        {
            List<string> allDomains = new List<string>();
            Forest CurrentForest = Forest.GetCurrentForest();
            DomainCollection MyDomains = CurrentForest.Domains;
            foreach(Domain obj in MyDomains)
            {
                allDomains.Add("@"+obj.Name);
            }
            return allDomains;
        }

        private void CreateAdUser(UserViewModel user)
        {
            UserPrincipal AdUser = new UserPrincipal(context);
            AdUser.DisplayName = AdUser.Name = user.FirstName + " " + user.LastName;
            AdUser.SamAccountName = user.UserLogonName;
            AdUser.UserPrincipalName = user.UserLogonName + user.UserDomain;
            AdUser.SetPassword(user.UserPassword);
            AdUser.Surname = user.LastName;
            AdUser.GivenName = user.FirstName;
            AdUser.Enabled = true;
            AdUser.ExpirePasswordNow();
            AdUser.Save();
        }

        private void GetAdUser(string _samaccountname, UserViewModel user)
        {
            TempData["samaccountname"] = _samaccountname;
            UserPrincipal _userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, _samaccountname);
            user.FirstName = _userPrincipal.GivenName;
            user.LastName = _userPrincipal.Surname;
            user.UserLogonName = _userPrincipal.SamAccountName;
            user.UserDomain = "@" + _userPrincipal.Context.Name + ".COM";
            user.DisplayName = _userPrincipal.Name;
            user.Samaccountname = _userPrincipal.SamAccountName;
        }


        // GET: User
        public ActionResult Index()
        {
            List<User> ADUsers = GetallAdUsers(context);
            return View(ADUsers);
        }

        // GET : Create User 
        public ActionResult Create()
        {
            UserViewModel user = new UserViewModel();
            user.Domains = EnumerateDomains();
            return View(user);
        }

        // Post : User which has been created
        [HttpPost]
        public ActionResult Create(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                CreateAdUser(user);
                return RedirectToAction("Index");
            }
            return View(User);
        }

        [HttpGet]
        public ActionResult Edit(string _samaccountname)
        {
            UserViewModel user = new UserViewModel();
            user.Domains = EnumerateDomains();
            if (_samaccountname == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GetAdUser(_samaccountname, user);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel user)
        {
            string _samaccountname = TempData["samaccountname"].ToString();
            //UserPrincipal AdUser = new UserPrincipal(context);
            UserPrincipal AdUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, _samaccountname);

            AdUser.Enabled = true;
            AdUser.DisplayName = user.FirstName +" "+ user.LastName;
            AdUser.SamAccountName = user.UserLogonName;
            AdUser.UserPrincipalName = user.UserLogonName + user.UserDomain;
            AdUser.Surname = user.LastName;
            AdUser.GivenName = user.FirstName;
            AdUser.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult ResetPassword(string _samaccountname)
        {
            if (_samaccountname == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserViewModel user = new UserViewModel();
            GetAdUser(_samaccountname, user);
            return View(user);
        }

        [HttpPost]
        public ActionResult ResetPassword(string NewPassword , UserViewModel user)
        {
            string _samaccountname = TempData["samaccountname"].ToString();
            UserPrincipal _userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, _samaccountname);
            user.Samaccountname = _userPrincipal.SamAccountName;
            if (user.Samaccountname == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else  
            {
                if (NewPassword != null)
                { 
                    _userPrincipal.Enabled = true;
                    _userPrincipal.SetPassword(NewPassword);
                    _userPrincipal.Save();
                    user.UserPassword = NewPassword;
                    return RedirectToAction(nameof(Index));
                }
                
            }

            return View(user);
        }

        public ActionResult Details(string _samaccountname)
        {
            if(_samaccountname == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserPrincipal _userPrincipal = new UserPrincipal(context);
            _userPrincipal.SamAccountName = _samaccountname;
            var searcher = new PrincipalSearcher(_userPrincipal);
            searcher.QueryFilter = _userPrincipal;
            var result = searcher.FindOne();
            User user = new User();

            
            user.UserName = result.UserPrincipalName;
            user.DisplayName = result.Name;
            user.Samaccountname = result.SamAccountName;
            
                    
            
            return View(user);
        }       
    }
}