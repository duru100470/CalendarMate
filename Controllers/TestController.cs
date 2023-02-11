using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly CalendarMate.Data.CalendarDbContext _context;

    public TestController(CalendarMate.Data.CalendarDbContext context, ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        var events = await _context.ApplicationUsers.ToListAsync();
        return Results.Ok(events);
    }

    [HttpPost]
    public async Task<IResult> Post(CalendarMate.Models.ApplicationUser _user)
    {
        var newUser = new CalendarMate.Models.ApplicationUser
        {
            UserName = "Duruchigy",
            Email = "zenpia@naver.com",
            PasswordHash = "n34u90th;sadlkfj23"
        };

        await _context.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return Results.Created($"/test/{newUser.UserId}", _user);
    }
}
