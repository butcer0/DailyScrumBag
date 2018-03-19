using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DailyScrumBag.Models;
using DailyScrumBag.Repository.Repositories;

namespace DailyScrumBag.Controllers
{
    public class HomeController : Controller
    {
        private readonly DSDBContext _db;

        public HomeController(DSDBContext db)
        {
            _db = db;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            var pageSize = 7;

            #region Retrieve Posts [Paginated]
            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Take(pageSize)
                    .ToArray();
            #endregion

            return View(posts);
        }

        #region Depricated - Unneeded
        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        #endregion
    }
}
