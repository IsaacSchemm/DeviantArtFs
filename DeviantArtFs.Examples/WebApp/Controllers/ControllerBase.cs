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
        protected readonly DeviantArtApp _appReg;

        public ControllerBase(ExampleDbContext context, DeviantArtApp appReg)
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
                    return new TokenWrapper(token, _appReg, _context);
                }
            }
            return null;
        }
    }
}
