using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeviantArtFs.Examples.StashInterface.Data;
using DeviantArtFs.Examples.StashInterface.Models;
using DeviantArtFs.Stash.Marshal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviantArtFs.Examples.StashInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly ExampleDbContext _context;
        private readonly DeviantArtAuth _appReg;

        public HomeController(ExampleDbContext context, DeviantArtAuth appReg)
        {
            _context = context;
            _appReg = appReg;
        }

        public IActionResult Index()
        {
            return View();
        }

        private async Task<IDeviantArtAccessToken> GetAccessTokenAsync()
        {
            if (HttpContext.Session.TryGetValue("token-id", out byte[] data))
            {
                var token = await _context.Tokens.SingleOrDefaultAsync(t => t.Id == new Guid(data));
                if (token.ExpiresAt < DateTimeOffset.UtcNow.AddMinutes(5))
                {
                    var result = await _appReg.RefreshAsync(token.RefreshToken);
                    token.AccessToken = result.AccessToken;
                    token.RefreshToken = result.RefreshToken;
                    token.ExpiresAt = result.ExpiresAt;
                    await _context.SaveChangesAsync();
                }
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<IActionResult> Login()
        {
            if (await GetAccessTokenAsync() != null)
            {
                return RedirectToAction("Index");
            }

            return Redirect($"https://www.deviantart.com/oauth2/authorize?response_type=code&client_id={_appReg.ClientId}&redirect_uri=https://localhost:5001/Home/Callback&scope=stash");
        }

        public async Task<IActionResult> Callback(string code, string state = null)
        {
            var result = await _appReg.GetTokenAsync(code, new Uri("https://localhost:5001/Home/Callback"));
            var token = new Token
            {
                Id = Guid.NewGuid(),
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt
            };
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();
            HttpContext.Session.Set("token-id", token.Id.ToByteArray());
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Whoami()
        {
            var t = await GetAccessTokenAsync();
            if (t == null)
                return RedirectToAction("Login");

            var me = await Requests.User.Whoami.ExecuteAsync(t);
            return Content(me.Username);
        }

        public async Task<IActionResult> FullRefresh()
        {
            var t = await GetAccessTokenAsync();
            if (t == null)
                return RedirectToAction("Login");

            var me = await Requests.User.Whoami.ExecuteAsync(t);
            var existingCursors = await _context.DeltaCursors
                .Where(x => x.UserId == me.Userid)
                .Select(x => x.Cursor)
                .ToListAsync();

            var existingItems = await _context.StashEntries
                .Where(x => x.UserId == me.Userid)
                .OrderBy(x => x.Position)
                .ToListAsync();

            _context.RemoveRange(existingCursors);
            _context.RemoveRange(existingItems);
            await _context.SaveChangesAsync();

            return await Refresh();
        }

        public async Task<IActionResult> Refresh()
        {
            var t = await GetAccessTokenAsync();
            if (t == null)
                return RedirectToAction("Login");

            var me = await Requests.User.Whoami.ExecuteAsync(t);
            var existingCursor = await _context.DeltaCursors
                .Where(x => x.UserId == me.Userid)
                .Select(x => x.Cursor)
                .SingleOrDefaultAsync();

            var existingItems = await _context.StashEntries
                .Where(x => x.UserId == me.Userid)
                .OrderBy(x => x.Position)
                .ToListAsync();
            var stashRoot = new StashRoot();
            foreach (var i in existingItems)
            {
                stashRoot.Apply(i);
            }

            var req = new Requests.Stash.DeltaRequest
            {
                Cursor = existingCursor
            };
            var paging = new PagingParams
            {
                Offset = 0,
                Limit = 120
            };

            while (true)
            {
                var delta = await Requests.Stash.Delta.ExecuteAsync(t, paging, req);
                existingCursor = delta.Cursor;
                if (delta.Reset)
                {
                    stashRoot.Clear();
                }
                foreach (var i in delta.Entries)
                {
                    stashRoot.Apply(i);
                }
                if (!delta.HasMore) break;
                paging.Offset = delta.NextOffset ?? 0;
            }

            _context.StashEntries.RemoveRange(existingItems);
            foreach (var new_entry in stashRoot.Save())
            {
                _context.StashEntries.Add(new StashEntry
                {
                    UserId = me.Userid,
                    ItemId = new_entry.Itemid,
                    StackId = new_entry.Stackid,
                    MetadataJson = new_entry.MetadataJson,
                    Position = new_entry.Position,
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ViewStack");
        }

        public async Task<IActionResult> ViewStack(long? stackid = null)
        {
            var t = await GetAccessTokenAsync();
            if (t == null)
                return RedirectToAction("Login");

            var me = await Requests.User.Whoami.ExecuteAsync(t);

            var existingItems = await _context.StashEntries
                .Where(x => x.UserId == me.Userid)
                .OrderBy(x => x.Position)
                .ToListAsync();
            var stashRoot = new StashRoot();
            foreach (var i in existingItems)
            {
                stashRoot.Apply(i);
            }

            var children = stackid is long s
                ? stashRoot.FindStackById(s).Children
                : stashRoot.Children;
            return Json(children);
        }
    }
}