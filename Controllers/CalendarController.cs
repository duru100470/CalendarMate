using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;
using CalendarMate.Data;
using CalendarMate.Models;
using CalendarMate.Services;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly CalendarDbContext _context;
    private readonly IAuthManager _authManager;

    public CalendarController(CalendarDbContext context, IAuthManager authManager)
    {
        _context = context;
        _authManager = authManager;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        var events = await _context.Events.ToListAsync();
        return Results.Ok(events);
    }

    [HttpPost]
    public async Task<IResult> Post(Event _event)
    {
        var auth = _authManager.CheckSession(Request, out var userId);
        if (auth != Results.Ok()) return auth;

        var newEvent = new Event
        {
            Title = _event.Title,
            Date = _event.Date,
            Description = _event.Description,
            UserId = userId ?? 0
        };

        await _context.AddAsync(newEvent);
        await _context.SaveChangesAsync();

        return Results.Created($"/calendar/{newEvent.EventId}", _event);
    }

    [HttpDelete("{id}")]
    public async Task<IResult> Delete(int id)
    {
        var auth = _authManager.CheckSession(Request, out var userId);
        if (auth != Results.Ok()) return auth;

        var _event = await _context.Events.FindAsync(id);
        if (_event == null)
            return TypedResults.NotFound();

        if (_event.UserId != userId) return Results.StatusCode(403);

        _context.Events.Remove(_event);
        await _context.SaveChangesAsync();
        return TypedResults.Ok(_event);
    }

    [HttpPut("{id}")]
    public async Task<IResult> Put(int id, Event inputEvent)
    {
        var auth = _authManager.CheckSession(Request, out var userId);
        if (auth != Results.Ok()) return auth;

        var _event = await _context.Events.FindAsync(id);

        if (_event is null) return TypedResults.NotFound();
        if (_event.UserId != userId) return Results.StatusCode(403);

        _event.Title = inputEvent.Title;
        _event.Date = inputEvent.Date;
        _event.Description = inputEvent.Description;

        await _context.SaveChangesAsync();

        return TypedResults.Created($"/calendar/{_event.EventId}", _event);
    }
}
