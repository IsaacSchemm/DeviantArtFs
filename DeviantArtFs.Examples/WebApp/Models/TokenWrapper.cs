using DeviantArtFs.Examples.WebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeviantArtFs.Examples.WebApp.Models
{
    public class TokenWrapper : IDeviantArtAutomaticRefreshToken
    {
        public IDeviantArtRefreshToken CurrentToken { get; private set; }

        private readonly Token _originalToken;
        private readonly IDeviantArtAuth _auth;
        private readonly ExampleDbContext _context;

        public TokenWrapper(Token originalToken, IDeviantArtAuth auth, ExampleDbContext context)
        {
            _originalToken = originalToken;
            _auth = auth;
            _context = context;

            CurrentToken = originalToken;
        }

        IDeviantArtAuth IDeviantArtAutomaticRefreshToken.DeviantArtAuth => _auth;
        DateTimeOffset IDeviantArtRefreshToken.ExpiresAt => CurrentToken.ExpiresAt;
        string IDeviantArtRefreshToken.RefreshToken => CurrentToken.RefreshToken;
        string IDeviantArtAccessToken.AccessToken => CurrentToken.AccessToken;

        public async Task UpdateTokenAsync(IDeviantArtRefreshToken value)
        {
            CurrentToken = value;

            var t = await _context.Tokens.FindAsync(_originalToken.Id);
            if (t != null)
            {
                t.AccessToken = value.AccessToken;
                t.RefreshToken = value.RefreshToken;
                t.ExpiresAt = value.ExpiresAt;
                await _context.SaveChangesAsync();
            }
        }
    }
}
