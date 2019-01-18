using DeviantArtFs.Examples.WebApp.Data;
using DeviantArtFs.Examples.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Controllers
{
    public abstract class ControllerBase : Controller
    {
        protected readonly ExampleDbContext _context;
        protected readonly DeviantArtAuth _appReg;

        public ControllerBase(ExampleDbContext context, DeviantArtAuth appReg)
        {
            _context = context;
            _appReg = appReg;
        }

        protected async Task<IDeviantArtAccessToken> GetAccessTokenAsync()
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
    }
}
