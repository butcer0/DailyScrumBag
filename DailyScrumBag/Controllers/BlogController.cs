using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyScrumBag.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly DSDBContext _db;

        public BlogController(DSDBContext db)
        {
            _db = db;
        }

        #region Depricated - Introduced Ajax Pagination
        //[Route("")] //overrides any default route matching, so must be exactly the route provided
        //public IActionResult Index()
        //{
        //    #region Depricated - Retrieve data from model
        //    // var posts = new[]
        //    // {
        //    //    new Post()
        //    // {
        //    //     Title = "My blog post",
        //    //     Posted = DateTime.Now,
        //    //     Author = "Erik Butcher",
        //    //     Body = "This is a great blog post"
        //    // },
        //    //     new Post()
        //    // {
        //    //     Title = "My first blog post",
        //    //     Posted = DateTime.Now - TimeSpan.FromHours(1),
        //    //     Author = "Erik Butcher",
        //    //     Body = "This is a great blog post that I posted an hour ago"
        //    // }
        //    //};
        //    #endregion

        //    var posts = _db.Posts.OrderByDescending(x => x.Posted).Take(5).ToArray();

        //    return View(posts);
        //}
        #endregion
        [Route("")]
        public IActionResult Index(int page = 0)
        {
            #region Caculate Pagination
            var pageSize = 2;
            var totalPosts = _db.Posts.Count();
            var totalPages = totalPosts / pageSize;
            var previousPage = page - 1;
            var nextPage = page + 1;

            ViewBag.PreviousPage = previousPage;
            ViewBag.HasPreviousPage = previousPage >= 0;
            ViewBag.NextPage = nextPage;
            ViewBag.HasNextPage = nextPage < totalPages;
            #endregion

            #region Retrieve Posts [Paginated]
            var posts =
                _db.Posts
                    .OrderByDescending(x => x.Posted)
                    .Skip(pageSize * page)
                    .Take(pageSize)
                    .ToArray();
            #endregion

            #region Check Header for Ajax Call
            #region Depricated - Ajax returning XMLHttpRequest not XMLH
            //if (Request.Headers["X-Requested-With"] == "XMLH")
            #endregion
            if (Request.Headers["X-Requested-With"] == "XMLH"
            || Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView(posts);
            }
            #endregion


            return View(posts);
        }

        [Route("{year:min(2000)}/{month:int}/{key}")]
        public IActionResult Post(int year, int month, string key)
        {
            //return new ContentResult { Content = string.Format("Month:{0}, Year:{1}, Title:{2}", month, year, key) };
            #region Depricated - Use Post from DbContext
            //Post post = new Post()
            //{
            //    Title = "My blog post",
            //    Posted = DateTime.Now,
            //    Author = "Erik Butcher",
            //    Body = "This is a great blog post"
            //};
            #endregion
            Post post = _db.Posts.Where(c => c.Key == key).FirstOrDefault();

            return View(post);
        }

        [Authorize]
        [HttpGet, Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        #region Depricated - More verbose solution
        //[HttpPost, Route("create")]
        //public IActionResult Create(CreatePostRequest post)
        //{
        //    return View();
        //}

        //public class CreatePostRequest
        //{
        //    public string Title { get; set; }
        //    public string Body { get; set; }
        //}
        #endregion

        [Authorize]
        [HttpPost, Route("create")]
        public IActionResult Create(Post post)
        {
            //Erik - 10/6/2017 Validate, input form validated data is valid
            if (!ModelState.IsValid)
            {
                return View();
            }

            #region Overriding Author and Posted Values to disallow setting the properties manually upstream
            post.Author = User.Identity.Name;
            post.Posted = DateTime.Now;
            #endregion

            _db.Posts.Add(post);
            _db.SaveChanges();
            #region Depricated - RedirectToAction
            //return View();
            #endregion
            return RedirectToAction("Post", "Blog", new
            {
                year = post.Posted.Year,
                month = post.Posted.Month,
                key = post.Key
            });
        }


    }

    #region Depricated - Introduced Controller Scope Route Attribute
    //public class BlogController : Controller
    //{
    //    // GET: /<controller>/
    //    public IActionResult Index()
    //    {
    //        //return View();
    //        return new ContentResult { Content = "Blog posts" };
    //    }

    //    [Route("blog/{year:min(2000)}/{month:int}/{key}")]
    //    public IActionResult Post(int year, int month, string key)
    //    {
    //        #region Depricated - Introduced Attribute Routing
    //        //if(!id.HasValue)
    //        //{
    //        //    //return BadRequest();

    //        //    //return RedirectToRoute("404");
    //        //    return RedirectToRoute(new
    //        //    {
    //        //        controller = "404.html",
    //        //        action = "Index"
    //        //    });
    //        //}
    //        //return new ContentResult { Content = id.ToString() };
    //        #endregion

    //        return new ContentResult { Content = string.Format("Month:{0}, Year:{1}, Title:{2}", month, year, key) };
    //    }
    //}
    #endregion
}
