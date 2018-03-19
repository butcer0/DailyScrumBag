using DailyScrumBag.Repository.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DailyScrumBag.ViewComponents
{
    [ViewComponent]
    public class FeaturedSlideViewComponent : ViewComponent
    {
        private readonly DSDBContext _db;

        public FeaturedSlideViewComponent(DSDBContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            IEnumerable<Repository.Models.Post> currentFeaturedSlides = _db.GetFeaturedSlides();

            return View(currentFeaturedSlides);
        }
    }
}
