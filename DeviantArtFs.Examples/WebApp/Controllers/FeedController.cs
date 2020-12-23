using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantArtFs.Examples.WebApp.Data;
using DeviantArtFs.Examples.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeviantArtFs.Examples.WebApp.Controllers
{
    public class FeedController : ControllerBase
    {
        public FeedController(ExampleDbContext context, DeviantArtApp appReg) : base(context, appReg) { }

        public IActionResult Index(string cursor = null)
        {
            return View();
        }

        public async Task<IActionResult> Pull(string cursor = null)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return Forbid();

            var feed = await Api.Feed.FeedHome.ExecuteAsync(token, cursor);
            return Json(new FeedModel(feed));
        }
    }
}