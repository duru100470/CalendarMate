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
    private readonly Dictionary<Guid, ApplicationUser> session = new Dictionary<Guid, ApplicationUser>();

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
            var ssid = Guid.NewGuid();

            session[ssid] = user;
            Console.WriteLine($"GUID: {ssid.ToString()}, Username: {user.UserName}");

            Response.Cookies.Append("Auth", ssid.ToString(), new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1)
            });
            foreach (var kv in session)
            {
                Console.WriteLine($"{kv.Key.ToString()}");
            }
            return Results.Ok();
        }
    }

    [Route("logout")]
    [HttpPost]
    public IResult Logout()
    {
        var ssid = Request.Cookies["Auth"];
        if (ssid == null) return Results.BadRequest();

        var ret = session.Remove(Guid.Parse(ssid));

        if (ret)
        {
            Console.WriteLine($"SSID {ssid} has been deleted.");
            Response.Cookies.Delete("Auth");
            return Results.Ok();
        }
        else
        {
            return Results.NotFound();
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
