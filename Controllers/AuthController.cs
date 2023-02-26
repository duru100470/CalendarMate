using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;
using CalendarMate.Models;
using System.Security.Cryptography;
using System.Text;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly CalendarMate.Data.CalendarDbContext _context;

    public AuthController(CalendarMate.Data.CalendarDbContext context)
    {
        _context = context;
    }

    [Route("test")]
    [HttpGet]
    public string Test(ApplicationUser _user)
    {
        return "Hello World!";
    }

    [Route("register")]
    [HttpPost]
    public async Task<IResult> Register(ApplicationUser _user)
    {
        var existUser = from u in _context.ApplicationUsers
            where u.Email == _user.Email || u.UserName == _user.UserName
            select u;

        if (existUser.Count() > 0) return Results.Conflict();

        var password = _user.PasswordHash;
        string passwordHash = GetSHA256(password);

        var newUser = new ApplicationUser
        {
            UserName = _user.UserName,
            Email = _user.Email,
            PasswordHash = passwordHash
        };

        await _context.ApplicationUsers.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Results.Created($"/auth/register/{newUser.UserId}", newUser);
    }

    [Route("login")]
    [HttpPost]
    public IResult Login(ApplicationUser _user)
    {
        var user = _context.ApplicationUsers.Where(u => u.Email == _user.Email).FirstOrDefault();

        if (user == null) return Results.NotFound();
        else if (user.PasswordHash != GetSHA256(_user.PasswordHash)) return Results.Unauthorized();
        else
        {
            Response.Cookies.Append("CalendarMate", user.PasswordHash, new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            });
            return Results.Ok();
        }
    }

    private string GetSHA256(string text)
    {
        using (SHA256 sha = SHA256.Create())
        {
            var origin = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(origin);
            return Convert.ToBase64String(hash);
        }
    }
}
