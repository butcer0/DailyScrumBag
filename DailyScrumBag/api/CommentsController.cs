using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DailyScrumBag.Repository.Models;
using DailyScrumBag.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DailyScrumBag.api
{
    [Route("api/posts/{postKey}/comments")]
    public class CommentsController : Controller
    {
        private readonly DSDBContext _db;
        //Erik - 10/9/2017 BlogDataContext IOC injection
        public CommentsController(DSDBContext db)
        {
            _db = db;
        }

        // GET: api/values
        [HttpGet]
        public IQueryable<Comment> Get(string postKey)
        {
            #region Depricated - Don't Allow returning all Comments
            //return _db.Comments;
            #endregion

            return _db.Comments.Where(x => x.Post.Key == postKey);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Comment Get(long id)
        {
            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
            return comment;
        }

        // POST api/values
        [HttpPost]
        #region Depricated - Post Comment to specific postKey
        //public void Post([FromBody]string value)
        #endregion
        public Comment Post(string postKey, [FromBody]Comment comment)
        {
            #region Retrieve associated Post
            var post = _db.Posts.FirstOrDefault(x => x.Key == postKey);
            #endregion

            if (post == null)
            {
                return null;
            }

            #region If Post exists, associate post, set posted date and author then save to database
            comment.Post = post;
            comment.Posted = DateTime.Now;
            comment.Author = User.Identity.Name;

            _db.Comments.Add(comment);
            _db.SaveChanges();
            #endregion

            //Erik - 10/9/2017 Return persisted comment to api caller
            return comment;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        #region Depricated - Update the entire Comment object
        //public void Put(int id, [FromBody]string value)
        #endregion
        public IActionResult Put(long id, [FromBody]Comment updated)
        {
            var comment = _db.Comments.Where(c => c.Id == id).FirstOrDefault();
            if (comment == null)
            {
                return NotFound();
            }

            comment.Body = updated.Body;
            _db.SaveChanges();

            return Ok();
        }

        #region Depricated - Return Refresh
        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(long id)
        //{
        //    #region Depricated - Check Null 
        //    //_db.RemoveRange(_db.Comments.Where(c => c.Id == id));
        //    #endregion
        //    var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
        //    if(comment != null)
        //    {
        //        _db.Comments.Remove(comment);
        //        _db.SaveChanges();
        //    }

        //    //TODO: Erik - 10/9/2017 Call Get to return updated comments on postKey

        //}

        #endregion

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IQueryable<Comment> Delete(string postKey, long id)
        {

            var comment = _db.Comments.FirstOrDefault(x => x.Id == id);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
                _db.SaveChanges();
            }

            return Get(postKey);

        }
    }
}
