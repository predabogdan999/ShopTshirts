using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShopTshirts.Identity;
using ShopTshirts.Models;

namespace ShopTshirts.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthController(
               UserManager<IdentityUser> userManager,
               RoleManager<IdentityRole> roleManager,
               IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            _configuration = configuration;
        }


        // Login
        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (model == null)
            {
                return BadRequest("Invalid client request");
            }
            var user = await _userManager.FindByNameAsync(model.Username);
           
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                

                  var claims = await GetValidClaims(user);

                var token = GetToken(claims);
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { Token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }
        private JwtSecurityToken GetToken(List<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                   issuer: "http://localhost:5000",
                   audience: "http://localhost:5000",
                   claims: claims,
                   expires: DateTime.Now.AddMinutes(20),
                   signingCredentials: signinCredentials
               );
            
            return tokeOptions;
        }

        //Register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                  Email = model.Email,
                PasswordHash = model.Password,
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
         
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed!Please check user details and try again." });
               
            }
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }


        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed!Please check user details and try again." });
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
               /* var adminRole = await _roleManager.FindByNameAsync(UserRoles.Admin);

                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Admin.Add));

                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Admin.Edit));

                await _roleManager.AddClaimAsync(adminRole, new Claim(CustomClaimTypes.Permission, Admin.Delete));*/
             
                 await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, UserRoles.Admin));
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        [HttpPost]
        [Route("register-editor")]
        public async Task<IActionResult> RegisterEditor([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed!Please check user details and try again." });
           if (!await _roleManager.RoleExistsAsync(UserRoles.Editor))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Editor));
            
            if (await _roleManager.RoleExistsAsync(UserRoles.Editor))
            {
                // var editorRole = await _roleManager.FindByNameAsync(UserRoles.Editor);
                //   await _roleManager.AddClaimAsync(editorRole, new Claim(CustomClaimTypes.Permission, Editor.AddProd));
                await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, UserRoles.Editor));
                await _userManager.AddToRoleAsync(user, UserRoles.Editor);
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        private async Task<List<Claim>> GetValidClaims(IdentityUser user)
        {
             var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
             {
                new Claim("Id", user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(roleClaim);
                    }
                }
            }
          
            return claims;
        }



    }
}