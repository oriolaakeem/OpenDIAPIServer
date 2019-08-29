using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace ChapelHill
{
    public class ServerTokenComponent : IServerTokenComponent
    {
        private readonly IHttpContextAccessor _context;

        public ServerTokenComponent(IConfiguration configuration, IHostingEnvironment environment, IHttpContextAccessor context)
        {
            Configuration = configuration;
            Environment = environment;
            _context = context;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        //private TokenResponse TokenResponse { get; set; }
        private string Token { get; set; }
        private DateTime? ExpiryTime { get; set; }

        public async Task<string> RequestTokenAsync()
        {
            var now = Environment.IsDevelopment() ? DateTime.Now : DateTime.UtcNow;
            Token = await _context.HttpContext.GetTokenAsync("access_token");
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(Token);
            var exp = jwtToken.Claims.Where(c => c.Type == "exp").FirstOrDefault().Value;
            var expiryTime = double.Parse(exp).UnixTimeStampToDateTime();
            if (expiryTime > now)
                return Token;
            else
            {
                Token = string.Empty;
                string referer = _context.HttpContext.Request.Headers["Referer"].ToString();

                //log out user first
                await _context.HttpContext.SignOutAsync("Cookies");
                await _context.HttpContext.SignOutAsync("oidc");


                await _context.HttpContext.ChallengeAsync("oidc", new AuthenticationProperties { RedirectUri = referer });

                return Token;
            }
        }

        public async Task<string> GetTokenAsync()
        {
            string token = "";
            foreach (var item in _context.HttpContext.Request.Headers)
            {
                if (item.Key == "Authorization")
                {
                    token = item.Value.ToString().Replace("Bearer ", "");
                }
            }
            return token;
        }
    }
}
