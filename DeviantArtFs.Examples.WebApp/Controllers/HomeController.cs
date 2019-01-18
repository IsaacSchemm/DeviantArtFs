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

        private async Task<Token> GetAccessTokenAsync()
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
            }
            return null;
        }

        public async Task<IActionResult> Login()
        {
            if (await GetAccessTokenAsync() != null)
            {
                return RedirectToAction("Index");
            }
            return Redirect($"https://www.deviantart.com/oauth2/authorize?response_type=code&client_id={_appReg.ClientId}&redirect_uri=https://{HttpContext.Request.Host}/Home/Callback&scope=browse");
        }

        public async Task<IActionResult> Callback(string code, string state = null)
        {
            var result = await _appReg.GetTokenAsync(code, new Uri($"https://{HttpContext.Request.Host}/Home/Callback"));
            var me = await Requests.User.Whoami.ExecuteAsync(result);
            var token = new Token
            {
                Id = Guid.NewGuid(),
                UserId = me.Userid,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresAt = result.ExpiresAt
            };
            _context.Tokens.Add(token);
            await _context.SaveChangesAsync();

            var claimsIdentity = new ClaimsIdentity(
                new[] {
                    new Claim(ClaimTypes.Name, me.Username),
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

        public async Task<IActionResult> Logout()
        {
            var t = await GetAccessTokenAsync();
            if (t != null)
            {
                _context.Tokens.RemoveRange(_context.Tokens.Where(x => x.Id == t.Id));
                await _context.SaveChangesAsync();
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index");
        }
    }
}