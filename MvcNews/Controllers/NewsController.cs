using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MvcNews.Models;
using MvcNews.DBAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace MvcNews.Controllers
{
    /// <summary>
    /// This controller class handles the interaction between the model and view layer for news.
    /// </summary>
    public class NewsController : Controller
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="iconfig">Config parameter used for database access.</param>
        public NewsController(IConfiguration iconfig)
        {
            _iconfig = iconfig;
        }

        // Config variable:
        IConfiguration _iconfig;


        public IActionResult Index(int page = 0)
        {
            if (isLoggedIn())
            {
                try
                {
                    List<News> newslist = new DBConnect(_iconfig).getAllEntries();

                    if (TempData["success"] != null)
                        ViewBag.Success = TempData["success"];
                    if (TempData["error"] != null)
                        ViewBag.Error = TempData["error"];

                    if (!newslist.Any())
                        ViewBag.Msg = "There are currently no entries...";

                    // Number of entries to be displayed per page:
                    int totalpages = Convert.ToInt16(_iconfig.GetSection("EntriesPerPage").GetSection("Items").Value);
                    var items = from e in newslist orderby e.EntryDate descending select e;
                    // Fetch total numbers of items in db:
                    var rowcount = items.Count();
                    // Read limited number of items per page and enable navigation to next page:
                    var entriesperpage = items.Skip(page * totalpages).Take(totalpages).ToList();
                    int entrymaxpage = (rowcount / totalpages) - (rowcount % totalpages == 0 ? 1 : 0);

                    if (page > entrymaxpage)
                        page = entrymaxpage;

                    ViewBag.maxpage = entrymaxpage;
                    ViewBag.page = page;

                    if (!items.Any())
                        ViewBag.Msg = "There are no entries yet...";

                    return View(entriesperpage.ToList<News>());
                }
                catch (Exception e)
                {
                    ViewBag.Error = "DB message: " + e.Message;
                }
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method returns a list page for an admin to edit news.
        /// </summary>
        /// <returns>News list.</returns>
        [HttpPost]
        public IActionResult Index(string filter, int check)
        {
            if (isLoggedIn())
            {
                try
                {
                    List<News> newslist = new DBConnect(_iconfig).getAllEntries();

                    if ((!filterList(newslist, $"{filter}", 0).Any() && $"{filter}" != null)
                    || (!filterList(newslist, null, Convert.ToInt16($"{check}")).Any() && Convert.ToInt32($"{check}") != 0))
                        ViewBag.Msg = "There were no matching hits...";

                    return View(filterList(newslist, $"{filter}", Convert.ToInt32($"{check}")));
                }
                catch (Exception e)
                {
                    ViewBag.Error = "DB message: " + e.Message;
                }
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method gets the form to create news based on user permission.
        /// </summary>
        /// <returns>Create page for news.</returns>
        public IActionResult Create()
        {
            News model = new News();

            if (isLoggedIn())
            {
                // Set default value of date to current date:
                model.EntryDate = DateTime.Now.ToString("yyyy-MM-dd");

                // Populate the dropdown status list and set default value:
                populateStatusDropdownlist("Published");

                // Return the page to be displayed to the end user:
                return View(model);
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method renders the create news page on a post request.
        /// </summary>
        /// <param name="model">News object.</param>
        /// <returns>Create news page after submit.</returns>
        [HttpPost]
        public IActionResult Create(News model)
        {
            // Catch email session, if it's set:
            string userEmail = HttpContext.Session.GetString("email");
            ViewBag.Email = userEmail;

            // Check valid login:
            if (userEmail != null)
            {
                // Assign/attach user email to object before db save:
                model.Email = userEmail;

                if (model.EntryDate != null)
                {
                    // Add hours and minutes to posted date (since it's not attached from form input):
                    string date = Convert.ToString(model.EntryDate + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                    model.EntryDate = date;
                }
                else
                {
                    ViewBag.Error = "Please select a valid date format, e.g. '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

                    // Required re-population of dropdownlist in order to render the page properly:
                    populateStatusDropdownlist(model.Status);
                    return View();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        new DBConnect(_iconfig).createEntry(model);

                        // ViewBag variables to be reached in view layer:
                        ViewBag.Success = "The entry was saved successfully.";

                        // Clear inputs after successful submit:
                        model.Title = "";
                        ModelState.Clear();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "(DB message: " + ex.Message + ")";
                        return View();
                    }
                }
                // Required re-population of dropdownlist in order to render the page properly:
                populateStatusDropdownlist(model.Status);
                return View();
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method gets the edit form based on user permission.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        /// <returns>Entry ID.</returns>
        public IActionResult Edit(int? id)
        {
            News model = new News();

            if (isLoggedIn())
            {
                if (id == null)
                    return RedirectToAction("Index");

                if (ModelState.IsValid)
                {
                    try
                    {
                        model = new DBConnect(_iconfig).getEntry(Convert.ToInt32(id));

                        // Set default value of date to current date:
                        model.EntryDate = Convert.ToDateTime(model.EntryDate).ToString("yyyy-MM-dd");
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = "(DB message: " + e.Message + ")";
                        populateStatusDropdownlist(model.Status);
                        return View();
                    }
                }
                populateStatusDropdownlist(model.Status);
                return View(model);
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This action method renders the edit page on a post request.
        /// </summary>
        /// <param name="model">News object.</param>
        /// <returns>Edit page or login page depending on valid login.</returns>
        [HttpPost]
        public IActionResult Edit(News model)
        {
            if (isLoggedIn())
            {
                if (model.EntryDate != null)
                {
                    // Add hours and minutes to posted date (since it's not attached from form input):
                    string date = Convert.ToString(model.EntryDate + " " + DateTime.Now.Hour + ":" + DateTime.Now.Minute);
                    model.EntryDate = date;
                }
                else
                {
                    ViewBag.Error = "Please select a valid date format, e.g. '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";

                    // Required re-population of dropdownlist in order to render the page properly:
                    populateStatusDropdownlist(model.Status);
                    return View();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        // Update entry:
                        new DBConnect(_iconfig).editEntry(model);
                        ViewBag.Success = "The entry was successfully updated.";
                    }
                    catch (Exception e)
                    {
                        populateStatusDropdownlist(model.Status);
                        ViewBag.Error = "(DB message: " + e.Message + ")";
                        return View();
                    }
                }
                populateStatusDropdownlist(model.Status);
                return View(model);
            }
            else
                return RedirectToAction("Login", "Admin");
        }


        /// <summary>
        /// This method handles the entry delete action on a post request.
        /// </summary>
        /// <param name="id">Entry ID.</param>
        /// <returns>Delete</returns>
        public IActionResult Delete(int? id)
        {
            if (isLoggedIn())
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Prevent delete action if no entry is specified with the request:
                        if (id == null)
                            return RedirectToAction("Index");

                        new DBConnect(_iconfig).deleteEntry(Convert.ToInt32(id));

                        TempData["success"] = "The entry was successfully deleted.";
                        return RedirectToAction("Index");
                    }
                    catch (Exception e)
                    {
                        TempData["error"] = "(DB message: " + e.Message + ")";
                        return View();
                    }
                }
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Login", "Admin");
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


        /// <summary>
        /// This method uses LINQ to filter a news list based on a filter word input or selection.
        /// </summary>
        /// <param name="list">News list to filter.</param>
        /// <param name="filter">Filter word.</param>
        /// <returns></returns>
        private List<News> filterList(List<News> newslist, string filter, int checkbox = 0)
        {
            IEnumerable<News> list = null;

            // Filter depending on checkbox:
            switch (checkbox)
            {
                case 1: // Order by published:
                    list = from e in newslist
                           where e.Status.Equals("Published")
                           orderby e.EntryDate descending
                           select e;
                    break;
                case 2: // Order by drafts:
                    list = from e in newslist
                           where e.Status.Equals("Draft")
                           orderby e.EntryDate ascending
                           select e;
                    break;
                case 3: // Order by asc:
                    list = from e in newslist
                           orderby e.EntryDate descending
                           select e;
                    break;
                default: // Order by desc:
                    list = from e in newslist
                           orderby e.EntryDate descending
                           select e;
                    break;
            }

            // Filter by search word:
            if (!String.IsNullOrWhiteSpace(filter))
            {
                list = from e in newslist
                       where e.Title.ToUpper().Contains(filter.ToUpper()) || e.Entry.ToUpper().Contains(filter.ToUpper())
                       orderby e.EntryDate descending
                       select e;
            }

            // Convert linq list to the right news format:
            return list.ToList<News>();
        }


        /// <summary>
        /// This method gets the entry statuses from db to be able to populate a status
        /// dropdown list to be displayed on the create and edit entry page.
        /// </summary>
        /// <param name="selectedStatus"></param>
        private void populateStatusDropdownlist(object selectedStatus = null)
        {
            DBConnect db = new DBConnect(_iconfig);
            var statuslist = db.getEntryStatuses();

            // Save the list in a ViewBag to reach it in GUI:
            ViewBag.StatusList = new SelectList(statuslist, "Status", "Status", selectedStatus);
        }


    } // End class.

} // End namespace.