using DeviantArtFs;
using ExampleWebApp.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExampleWebApp.Models
{
    public class TokenWrapper : IDeviantArtRefreshableAccessToken
    {
        private readonly DeviantArtApp _app;
        private readonly ExampleDbContext _context;
        private Token _currentToken;

        public Guid IdInDatabase => _currentToken.Id;

        public string AccessToken => _currentToken.AccessToken;

        public string RefreshToken => _currentToken.RefreshToken;

        public TokenWrapper(Token originalToken, DeviantArtApp app, ExampleDbContext context)
        {
            _app = app;
            _context = context;
            _currentToken = originalToken;
        }

        public async Task RefreshAccessTokenAsync()
        {
            var newToken = await DeviantArtAuth.RefreshAsync(_app, _currentToken.RefreshToken);

            _currentToken =
                await _context.Tokens.FindAsync(_currentToken.Id) // if still in database
                ?? _currentToken; // if removed from database (no changes will be saved to database)
            _currentToken.AccessToken = newToken.access_token;
            _currentToken.RefreshToken = newToken.refresh_token;
            await _context.SaveChangesAsync();
        }
    }
}
