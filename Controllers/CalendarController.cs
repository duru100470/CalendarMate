using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly CalendarMate.Data.EventDbContext _events;

    public CalendarController(CalendarMate.Data.EventDbContext events, ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _events = events;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        var events = await _events.Events.ToListAsync();
        return Results.Ok(events);
    }

    [HttpPost]
    public async Task<IResult> Post(CalendarMate.Models.Event _event)
    {
        var newEvent = new CalendarMate.Models.Event
        {
            Title = _event.Title,
            Date = _event.Date,
            Description = _event.Description,
            UserId = _event.UserId
        };

        await _events.AddAsync(newEvent);
        await _events.SaveChangesAsync();

        return Results.Created($"/calendar/{newEvent.EventId}", _event);
    }
}
