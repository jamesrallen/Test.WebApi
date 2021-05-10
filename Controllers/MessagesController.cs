// Test.WebApi/Test.WebApi/MessagesController.cs
// James Allen
// 2021/05/10/3:23 PM

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Test.WebApi.Areas.Identity.Data;
using Test.WebApi.Data;
using Test.WebApi.DTOs;
using Test.WebApi.Models;

namespace Test.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly MessageDbContext _context;

        private readonly TestWebApiContext _testWebApiContext;
        private readonly UserManager<TestWebApiUser> _userManager;
        private readonly SignInManager<TestWebApiUser> _signInManager;



        public MessagesController(
            MessageDbContext context, 
            TestWebApiContext testWebApiContext, 
            UserManager<TestWebApiUser> userManager, 
            SignInManager<TestWebApiUser> signInManager
            )
        {
            _context = context;
            _testWebApiContext = testWebApiContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }





        // GET: api/Messages
        [HttpGet]
        [Authorize(Roles = Role.User)]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        // GET: api/Messages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Message>> GetMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null) return NotFound();

            return message;
        }


        // PUT: api/Messages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMessage(int id, MessageDto messageDto)
        {
            if (id != messageDto.Id) return BadRequest();

            //_context.Entry(messageDto).State = EntityState.Modified;


            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            message.UpdateFrom(messageDto);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MessageExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }


        // POST: api/Messages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Message>> CreateMessage(MessageDto messageDto)
        {
            var message = new Message(messageDto);
            
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetMessage", new { id = message.Id }, message);
            return CreatedAtAction(nameof(GetMessage), new { id = message.Id }, message);
        }


        // DELETE: api/Messages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null) return NotFound();

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return NoContent();
        }





        [HttpPost("getToken")]
        [AllowAnonymous]
        public async Task<IActionResult> GetToken(LoginModel loginModel)
        {
            var user = _testWebApiContext.Users.FirstOrDefault(x => x.Email == loginModel.Email);

            if (user != null)
            {
                //if (user.UserName == "user@test.com") user.Role = Role.User;
                if (user.UserName == "moderator@test.com") user.Role = Role.Moderator;
                if (user.UserName == "admin@test.com") user.Role = Role.Admin;
                if (string.IsNullOrEmpty(user.Role)) user.Role = Role.User;

                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, loginModel.Password, false);

                if (signInResult.Succeeded)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456");
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, loginModel.Email),
                            new Claim(ClaimTypes.Role, user.Role)
                        }),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256Signature)
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = tokenHandler.WriteToken(token);

                    return Ok(new { Token = tokenString });
                }

                return Ok("Login failed.");

            }
            return Ok("Login failed.");


            //if (loginModel.Email == "test@test.com" && loginModel.Password == "password")
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var key = Encoding.ASCII.GetBytes("qwertyuiopasdfghjklzxcvbnm123456");
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Subject = new ClaimsIdentity(new[]
            //        {
            //            new Claim(ClaimTypes.Name, loginModel.Email)
            //        }),
            //        Expires = DateTime.Now.AddDays(1),
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
            //            SecurityAlgorithms.HmacSha256Signature)
            //    };
            //    var token = tokenHandler.CreateToken(tokenDescriptor);
            //    var tokenString = tokenHandler.WriteToken(token);

            //    return Ok(new {Token = tokenString});
            //}

            //return Unauthorized("try again!");

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(LoginModel loginModel)
        {
            var testWebApiUser = new TestWebApiUser
            {
                Email = loginModel.Email,
                UserName = loginModel.Email,
                EmailConfirmed = false

            };

            var result = await _userManager.CreateAsync(testWebApiUser, loginModel.Password);

            if (result.Succeeded) return Ok(new {Result = "Register Success"});

            var stringBuilder = new StringBuilder();
            foreach (var identityError in result.Errors) stringBuilder.Append(identityError.Description);
            return Ok(new {Result = $"Register Fail: {stringBuilder}"});



        }

        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.Id == id);
        }
    }
}
