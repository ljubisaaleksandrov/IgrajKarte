using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using IgrajKarte.Models;
using IgrajKarte.Helpers;
using IgrajKarte.Enums;
using IgrajKarte.Filters;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Security;
using System.Security.Principal;

//test
namespace IgrajKarte.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/
        private EntityModel DataContext;
        private SessionManager SessionManager;

        public AccountController()
        {
            DataContext = new EntityModel();
            SessionManager = new SessionManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        //[AllowNoTerms]
        //[AllowNoVerify]
        public ActionResult LogOn(string returnUrl)
        {
            if (!String.IsNullOrEmpty(returnUrl))
            {
                //return RedirectToAction("LogOnWidget", "Account");
            }
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //return RedirectToAction("Index", "Transaction");
            }
            string t = HttpContext.User.Identity.AuthenticationType;
            bool b = HttpContext.User.Identity.IsAuthenticated;
            SessionManager.ReturnUrl = returnUrl;

            ViewBag.IsAuthenticated = false;
            
            return View();
        }

        [AllowAnonymous]
        //[AllowNoTerms]
        //[AllowNoVerify]
        [HttpPost]
        public ActionResult LogOn(LogOn entity)
        {
            if (string.IsNullOrEmpty(entity.UserName) | string.IsNullOrEmpty(entity.Password))
            {
                ModelState.AddModelError("InvalidEntry", "Username and/or Password is missing.");
                return View();
            }

            if (ModelState.IsValid)
            {
                User user = DataContext.Users.Where<User>(u => u.UserName == entity.UserName && u.Password == entity.Password).FirstOrDefault<User>();

                if (user == null)
                {
                    ModelState.AddModelError("Username", "User with specified credentials was not found. Please try again");
                    return View();
                }

                if (user.IsSuspended)
                {
                    ModelState.AddModelError("Username", "Your account has been suspended please contact someemail.com");
                    return View();
                }

                Session["CurrentUserId"] = user.Id;
                HttpContext.Session["CurrentUserId"] = user.Id;
                ViewBag.IsAuthenticated = true;

                if (user.Level == (int)UserAccessLevel.Member)
                    Session["isMember"] = true;
                else
                    Session["isAdministrator"] = true;

                FormsAuthentication.SetAuthCookie(entity.UserName, true);
                string t = HttpContext.User.Identity.AuthenticationType;
                bool b = HttpContext.User.Identity.IsAuthenticated;
                
                

                return RedirectToAction("Index", "DashBoard");
            }

            return View();
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();
            Session["CurrentUserId"] = -1;
            Session["isMember"] = false;
            Session["isAdministrator"] = false;

            return RedirectToAction("Index", "Home");
            //return RedirectToAction("LogIn", "Account");
        }

        [AllowAnonymous]
        //[AllowNoTerms]
        // [AllowNoVerify]
        public ActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        //[AllowNoTerms]
        //[AllowNoVerify]
        [HttpPost]
        //[SecureFilter]
        public ActionResult Register(Register entity)
        {
            if (entity.Terms == false)
            {
                return View(entity);
            }

            IQueryable<User> users = DataContext.Users.Where<User>(u => u.UserName == entity.UserName && u.Level == (int)UserAccessLevel.Member);

            if (users.Count<User>() != 0)
            {
                ViewBag.Error = "Error: Email already registered, try to <a href='" + Url.Action("ForgotPassword") + "'>retrieve password</a> or <a href='" + Url.Action("LogOn") + "'>login</a>.";

                return View(entity);
            }


            if (entity.Password != entity.ConfirmPassword)
            {
                ViewBag.Error = "Error: Password and Confirm Password fields doesn't match";

                return View(entity);
            }
            else
            {
                try
                {
                    // initialize new user
                    User newUser = new User();
                    newUser.Id = DataContext.Users.Count();
                    newUser.Name = entity.FirstName;
                    newUser.LastName = entity.LastName;
                    newUser.UserName = entity.UserName;
                    newUser.Password = entity.Password;
                    newUser.Email = entity.Email;
                    newUser.Address = entity.Address;
                    newUser.Phone = entity.Phone;
                    newUser.Level = (int)UserAccessLevel.Member;
                    newUser.IsVerified = false;
                    newUser.IsSuspended = false;
                    newUser.IsAutenticated = false;
                    newUser.DateCreated = DateTime.Now.ToString();
                    newUser.DateModified = DateTime.Now.ToString();

                    DataContext.Users.Add(newUser);


                    // initialize new user session data
                    LoginSession loginSession = new LoginSession();
                    loginSession.Id = DataContext.LoginSessions.Count();
                    loginSession.UserId = newUser.Id;
                    loginSession.Token = HttpContext.Session.SessionID;
                    loginSession.DateCreated = DateTime.Now.ToString();
                    loginSession.DateModified = DateTime.Now.ToString();

                    DataContext.LoginSessions.Add(loginSession);


                    // initialize new user log record

                    Log log = new Log();
                    log.UserId = newUser.Id;
                    log.SessionId = loginSession.Id;
                    log.DateCreated = DateTime.Now.ToString();

                    DataContext.SaveChanges();
                }
              
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine(validationError.PropertyName);
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }


                return RedirectToAction("Index", "Home");
            }
        }


        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }


        public ActionResult Contact()
        {
            return View();
        }
    }
}
