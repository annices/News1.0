using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using MvcNews.Models;
using MvcNews.DBAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;

namespace MvcNews.Controllers
{
    /// <summary>
    /// This controller class handles the interaction between the model and view layer for admins actions.
    /// </summary>
    public class AdminController : Controller
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iconfig">Config parameter used for database access.</param>
        public AdminController(IConfiguration iconfig)
        {
            _iconfig = iconfig;
        }

        // Config variable used to connect to db:
        IConfiguration _iconfig;


        /// <summary>
        /// This action method renders the create admin page. In this app, this page is ment as a one time action.
        /// </summary>
        /// <returns>Form to create a new admin user.</returns>
        public IActionResult Create()
        {
            // Check if page has been deleted:
            string path = "./Views/Admin/Create.cshtml";
            FileInfo file = new FileInfo(path);

            if (file.Exists)
                return View();

            else
                return RedirectToAction("Index", "Home");
        }


        /// <summary>
        /// This action method renders the post page when creating an admin.
        /// </summary>
        /// <param name="model">Admin object.</param>
        /// <returns>The create page on post request.</returns>
        [HttpPost]
        public IActionResult Create(Admin model)
        {
            // Handle exception if invalid model state:
            if (ModelState.IsValid)
            {
                try
                {
                    new DBConnect(_iconfig).createUser(model);

                    // For security reasons, delete the create user page after admin has been created:
                    string path = "./Views/Admin/Create.cshtml";
                    FileInfo file = new FileInfo(path);

                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    ModelState.Clear();
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ViewBag.Success = false;
                    ViewBag.Msg = ex.Message;
                }
            }
            return View(model);
        }


        /// <summary>
        /// This action method gets the admin edit form.
        /// </summary>
        /// <param name="email">Input to get specific user.</param>
        /// <returns>Admin object.</returns>
        public IActionResult Index()
        {
            string userEmail = HttpContext.Session.GetString("email");
            ViewBag.Email = userEmail;

            Admin model = new Admin();

            // Check valid login based on set email session:
            if (userEmail != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        model = new DBConnect(_iconfig).getUser(userEmail);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Msg = "(DB message: " + e.Message + ")";
                        return View();
                    }
                }
                return View(model);
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method renders the admin edit page on a post request.
        /// </summary>
        /// <param name="model">Admin object.</param>
        /// <returns>Edit page or login page depending on valid login.</returns>
        [HttpPost]
        public IActionResult Index(Admin model)
        {
            // Check valid login based on set email session:
            if (isLoggedIn())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Get current user password hash and salt:
                        Admin userPassword = new Admin();
                        userPassword = new DBConnect(_iconfig).getPassword(model.Email);

                        model.PasswordHash = userPassword.PasswordHash;
                        model.Salt = userPassword.Salt;

                        // Update user credentials:
                        new DBConnect(_iconfig).editUser(model);
                        ViewBag.Success = true;
                        ViewBag.Msg = "The user was updated successfully.";
                    }
                    catch (Exception e)
                    {
                        ViewBag.Success = false;
                        ViewBag.Msg = "(DB message: " + e.Message + ")";
                        return View(model);
                    }
                }
                return View(model);
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method gets the login form for an admin.
        /// </summary>
        /// <returns>The login page.</returns>
        public IActionResult Login()
        {
            return View();
        }


        /// <summary>
        /// This action method renders the admin login page on post request.
        /// </summary>
        /// <returns>The login page.</returns>
        [HttpPost]
        public IActionResult Login(Admin model)
        {
            if(String.IsNullOrEmpty(model.Password) || String.IsNullOrEmpty(model.Email))
            {
                ViewBag.Msg = "Invalid login!";
                return View();
            }

            // Handle exception if invalid model state:
            if (ModelState.IsValid)
            {
                try
                {
                    // If valid login, set session to user email to remember the login in other views:
                    if (new DBConnect(_iconfig).checkLogin(model.Email, model.Password))
                    {
                        HttpContext.Session.SetString("email", model.Email);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Msg = "Invalid login!";
                        return View(model);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Msg = e.Message;
                }

            }
            return View(model);
        }


        /// <summary>
        /// This action method is called to logout a user.
        /// </summary>
        /// <returns>Redirection to startpage</returns>
        public IActionResult Logout()
        {
            // Clear sessions to make a login invalid/logout user:
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }


        /// <summary>
        /// This method is used to validate a login based on if a session is set by a logged in user email.
        /// </summary>
        /// <param name="email">String used to pass a session.</param>
        /// <returns></returns>
        public Boolean isLoggedIn()
        {
            Boolean loggedIn;

            if (HttpContext.Session.GetString("email") != null)
            {
                ViewBag.Email = HttpContext.Session.GetString("email");
                loggedIn = true;
            }
            else
                loggedIn = false;

            return loggedIn;
        }


    } // End class.

} // End namespace.