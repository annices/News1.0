using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MvcNews.DBAccess;
using MvcNews.Models;

namespace MvcNews.Controllers
{
    public class HomeController : Controller
    {

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iconfig">Config parameter used for database access.</param>
        public HomeController(IConfiguration iconfig)
        {
            _iconfig = iconfig;
        }

        // Config variable:
        IConfiguration _iconfig;


        /// <summary>
        /// This action method renders a public start page to display the latest news.
        /// </summary>
        /// <returns>Public start page with latest news.</returns>
        public IActionResult Index(int page = 0)
        {
            // Catch email session, if there is any, to assign a ViewBag called in view:
            string userEmail = HttpContext.Session.GetString("email");
            ViewBag.Email = userEmail;

            List<News> newslist = new DBConnect(_iconfig).getAllEntries();

            if (!newslist.Any())
                ViewBag.Msg = "There are no published entries yet...";

            // Number of entries to be displayed per page:
            int totalpages = Convert.ToInt16(_iconfig.GetSection("EntriesPerPage").GetSection("Items").Value);
            var items = from e in newslist where e.Status.Equals("Published") orderby e.EntryDate descending select e;
            // Fetch total numbers of items in db:
            var rowcount = items.Count();
            // Read limited number of items per page and enable navigation to next page:
            var entriesperpage = items.Skip(page * totalpages).Take(totalpages).ToList();
            int entrymaxpage = (rowcount / totalpages) - (rowcount % totalpages == 0 ? 1 : 0);

            if (page > entrymaxpage)
                page = entrymaxpage;

            ViewBag.maxpage = entrymaxpage;
            ViewBag.page = page;

            return View(entriesperpage.ToList<News>());
        }


        /// <summary>
        /// This action method renders a detail view to be able to read a full entry.
        /// </summary>
        /// <returns></returns>
        public IActionResult Detail(int? id)
        {
            // Catch email session, if there is any, to assign a ViewBag called in view:
            string userEmail = HttpContext.Session.GetString("email");
            ViewBag.Email = userEmail;

            News model = new News();

            if (id == null)
                return RedirectToAction("Index");

            try
            {
                model = new DBConnect(_iconfig).getEntry(Convert.ToInt32(id));
                Admin a = new DBConnect(_iconfig).getUser(model.Email);
                ViewBag.UserFirstname = a.Firstname;

                model.EntryDate = model.EntryDate.Substring(0, 16);
            }
            catch (Exception e)
            {
                ViewBag.Error = "(DB message: " + e.Message + ")";
                return View();
            }
            return View(model);
        }


    } // End class.

} // End namespace.
