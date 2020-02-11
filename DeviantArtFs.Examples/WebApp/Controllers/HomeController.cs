using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DeviantArtFs.Examples.WebApp.Data;
using DeviantArtFs.Examples.WebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeviantArtFs.Examples.WebApp.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(ExampleDbContext context, DeviantArtApp appReg) : base(context, appReg) { }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login()
        {
            if (await GetAccessTokenAsync() != null)
            {
                return RedirectToAction("Index");
            }
            return Redirect($"https://www.deviantart.com/oauth2/authorize?response_type=code&client_id={_appReg.client_id}&redirect_uri=https://{HttpContext.Request.Host}/Home/Callback&scope=browse+feed+user");
        }

        public async Task<IActionResult> Callback(string code, string state = null)
        {
            IDeviantArtRefreshToken result = await DeviantArtAuth.GetTokenAsync(_appReg, code, new Uri($"https://{HttpContext.Request.Host}/Home/Callback"));
            var me = await Requests.User.Whoami.ExecuteAsync(result);
            var token = new Token
            {
                Id = Guid.NewGuid(),
                UserId = me.userid,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken
            };
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            var claimsIdentity = new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.Name, me.username),
                    new Claim("token-id", token.Id.ToString())
                }, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Whoami()
        {
            var t = await GetAccessTokenAsync();
            if (t == null)
                return RedirectToAction("Login");

            var me = await Requests.User.Whoami.ExecuteAsync(t);
            return Json(me);
        }

        public async Task S()
        {
            string str = User.Claims
                .Where(c => c.Type == "token-id")
                .Select(c => c.Value)
                .FirstOrDefault();
            if (str != null && Guid.TryParse(str, out Guid tokenId))
            {
                var token = await _context.Tokens.SingleOrDefaultAsync(t => t.Id == tokenId);
                if (token != null)
                {
                    token.AccessToken = "test";
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
            var t = await GetAccessTokenAsync();
            if (t is Token dbToken)
            {
                _context.Tokens.RemoveRange(_context.Tokens.Where(x => x.Id == dbToken.Id));
                await _context.SaveChangesAsync();
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
                await DeviantArtAuth.RevokeAsync(dbToken.RefreshToken, revoke_refresh_only: true);
                return RedirectToAction("Index");
            }
            else
            {
                return Content($"The token returned by GetAccessTokenAsync() is not from the database");
            }
        }
    }
}