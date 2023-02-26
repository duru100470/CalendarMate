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
        string passwordHash;

        using (SHA256 sha = SHA256.Create())
        {
            var origin = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(origin);
            passwordHash = Convert.ToBase64String(hash);
        }

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
}
