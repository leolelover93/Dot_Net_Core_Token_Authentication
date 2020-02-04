using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotNet_Core_Token_Authentication.DataAccesLayer;
using DotNet_Core_Token_Authentication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotNet_Core_Token_Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private UserManager<ApplicationUser> userManager;
        private RoleManager<string> roleManager;
        private readonly ApplicationDbContext _context;
       
        public AuthController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            IList<string> Roles;
            ApplicationUser user;
            List<Claim> claims;


            user = await userManager.FindByEmailAsync(model.UserName);
            if (user == null)
                return null;
            else
                Roles = await userManager.GetRolesAsync(user);

            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {

                claims = new List<Claim>();
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
                claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                claims.Add(new Claim(JwtRegisteredClaimNames.GivenName, user.PhoneNumber));
                foreach (string role in Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                //    var claims = new[]
                //    {
                //        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                //        new Claim(JwtRegisteredClaimNames.GivenName, user.PhoneNumber),
                //};

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecureKey"));
                var token = new JwtSecurityToken(
                    issuer: "http://lefajele.le",
                    audience: "http://lefajele.le",
                    expires: DateTime.UtcNow.AddHours(5),
                    claims: claims,
                    signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

    }
}