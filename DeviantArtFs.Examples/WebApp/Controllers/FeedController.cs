using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantArtFs.Examples.WebApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace DeviantArtFs.Examples.WebApp.Controllers
{
    public class FeedController : ControllerBase
    {
        public FeedController(ExampleDbContext context, DeviantArtAuth appReg) : base(context, appReg) { }

        public async Task<IActionResult> Index(string cursor = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var feed = await Requests.Feed.FeedHome.ExecuteAsync(token, cursor);
            return View(feed);
        }
    }
}