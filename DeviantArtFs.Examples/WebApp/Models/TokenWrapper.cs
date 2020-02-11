using DeviantArtFs.Examples.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class TokenWrapper : IDeviantArtRefreshToken, IDeviantArtAutomaticRefreshToken
    {
        public IDeviantArtRefreshToken CurrentToken { get; private set; }
        public DeviantArtApp App { get; private set; }

        private readonly Token _originalToken;
        private readonly ExampleDbContext _context;

        public TokenWrapper(Token originalToken, DeviantArtApp app, ExampleDbContext context)
        {
            _originalToken = originalToken;
            _context = context;

            CurrentToken = originalToken;
            App = app;
        }

        public string AccessToken => CurrentToken.AccessToken;
        public string RefreshToken => CurrentToken.RefreshToken;

        public async Task UpdateTokenAsync(IDeviantArtRefreshToken value)
        {
            CurrentToken = value;

            var t = await _context.Tokens.FindAsync(_originalToken.Id);
            if (t != null)
            {
                t.AccessToken = value.AccessToken;
                t.RefreshToken = value.RefreshToken;
                await _context.SaveChangesAsync();
            }
        }
    }
}
