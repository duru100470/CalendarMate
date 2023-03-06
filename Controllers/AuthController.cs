using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;
using CalendarMate.Models;
using CalendarMate.Services;
using System.Security.Cryptography;
using System.Text;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly CalendarMate.Data.CalendarDbContext _context;
    private readonly ISessionStorage _session;
    private readonly IEmailSender _emailSender;

    public AuthController(CalendarMate.Data.CalendarDbContext context, ISessionStorage session, IEmailSender emailSender)
    {
        _context = context;
        _session = session;
        _emailSender = emailSender;
    }

    [Route("test")]
    [HttpGet]
    public string Test()
    {
        return GenerateToken(32);
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
            PasswordHash = passwordHash,
            EmailToken = GenerateToken(32)
        };

        _emailSender.SendMail(newUser.Email, "[CalendarMate] Verify your account", 
            $"Verification link => https://localhost:5000/auth/verify?email={newUser.Email}&token={newUser.EmailToken}");

        await _context.ApplicationUsers.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Results.Created($"/auth/register/{newUser.UserId}", newUser);
    }

    [Route("verify")]
    [HttpGet]
    public async Task<IResult> VerifyEmail([FromQuery(Name = "email")] string email, [FromQuery(Name = "token")] string token)
    {
        var user = await
        (
            from u in _context.ApplicationUsers
            where u.Email == email
            select u
        ).FirstOrDefaultAsync();

        if (user == null) return Results.NotFound();

        if (user.EmailToken == string.Empty)
        {
            user.EmailToken = GenerateToken(32);
            await _context.SaveChangesAsync();
            return Results.Problem();
        }

        if (user.EmailToken != token) return Results.Unauthorized();

        user.IsVerified = true;
        await _context.SaveChangesAsync();

        return Results.Ok();
    }

    [Route("forgot")]
    [HttpPost]
    public async Task<IResult> ForgotPassword(ApplicationUser _user)
    {
        var user = await
        (
            from u in _context.ApplicationUsers
            where (u.Email == _user.Email && u.UserName == _user.UserName)
            select u
        ).FirstAsync();

        if (user == null) return Results.NotFound();

        user.EmailToken = GenerateToken(32);
        await _context.SaveChangesAsync();

        _emailSender.SendMail(user.Email, "[CalendarMate] Recover your account", 
            $" Recovery link => https://localhost:5000/auth/recover?id={user.UserId}&token={user.EmailToken} Click this link to reset password to (password)");
        return Results.Ok();
    }

    [Route("recover")]
    [HttpGet]
    public async Task<IResult> RecoverPassword([FromQuery(Name = "id")] int id, [FromQuery(Name = "token")] string token)
    {
        var user = await _context.ApplicationUsers.FindAsync(id);

        if (user is null) return TypedResults.NotFound();
        if (user.EmailToken != token) return Results.Unauthorized();

        user.PasswordHash = GetSHA256("password");
        await _context.SaveChangesAsync();

        return Results.Accepted();
    }

    [Route("account")]
    [HttpGet]
    public async Task<IResult> GetAccount()
    {
        var ssid = Request.Cookies["sessionId"];
        if (!Guid.TryParse(ssid, out var guid)) return Results.BadRequest();
        if (!_session.TryGetUser(guid, out var userId)) return Results.NotFound();

        var user = await _context.ApplicationUsers.FindAsync(userId);
        if (user == null) return Results.NotFound();
        user.PasswordHash = "";
        user.EmailToken = "";

        return Results.Ok(user);
    }

    [Route("account")]
    [HttpPut]
    public async Task<IResult> PutAccount(ApplicationUser inputUser)
    {
        var ssid = Request.Cookies["sessionId"];
        if (ssid == null) return Results.Unauthorized();
        if (!Guid.TryParse(ssid, out var guid)) return Results.BadRequest();
        if (!_session.TryGetUser(guid, out var userId)) return Results.NotFound();

        var user = await _context.ApplicationUsers.FindAsync(userId);

        if (user == null) return Results.NotFound();
        user.UserName = inputUser.UserName;

        await _context.SaveChangesAsync();

        return Results.Created($"/auth/account/{user.UserId}", user);
    }

    [Route("pass")]
    [HttpPut]
    public async Task<IResult> PutPassword(ApplicationUser inputUser)
    {
        // inputUser.UserName: new password
        // inputUser.PasswordHash = previous password
        var ssid = Request.Cookies["sessionId"];
        if (ssid == null) return Results.Unauthorized();
        if (!Guid.TryParse(ssid, out var guid)) return Results.BadRequest();
        if (!_session.TryGetUser(guid, out var userId)) return Results.NotFound();

        var user = await _context.ApplicationUsers.FindAsync(userId);

        if (user == null) return Results.NotFound();
        if (GetSHA256(inputUser.PasswordHash) != user.PasswordHash) return Results.Forbid();
        user.PasswordHash = GetSHA256(inputUser.UserName);

        await _context.SaveChangesAsync();

        return Results.Created($"/auth/account/{user.UserId}", user);
    }

    [Route("login")]
    [HttpPost]
    public IResult Login(ApplicationUser _user)
    {
        var user = _context.ApplicationUsers.Where(u => u.Email == _user.Email).FirstOrDefault();

        if (user == null) return Results.NotFound();
        else if (user.PasswordHash != GetSHA256(_user.PasswordHash)) return Results.Unauthorized();
        else if (!user.IsVerified) return Results.Forbid();
        else
        {
            _session.AddUser(user.UserId, out var ssid);

            Response.Cookies.Append("sessionId", ssid.ToString(), new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true
            });

            return Results.Ok();
        }
    }

    [Route("logout")]
    [HttpGet]
    public IResult Logout()
    {
        var ssid = Request.Cookies["sessionId"];
        if (ssid == null) return Results.BadRequest();

        if (_session.RemoveUser(Guid.Parse(ssid)))
        {
            Console.WriteLine($"SSID {ssid} has been deleted.");
            Response.Cookies.Delete("sessionId", new CookieOptions
            {
                SameSite = SameSiteMode.None,
                Secure = true,
                HttpOnly = true
            });
            return Results.Ok();
        }
        else
        {
            return Results.NotFound();
        }
    }

    private string GenerateToken(int size)
    {
        Random random = new Random();
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, size)
        .Select(s => s[random.Next(s.Length)]).ToArray());
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
