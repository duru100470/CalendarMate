﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CalendarMate;

namespace CalendarMate.Controllers;

[ApiController]
[Route("[controller]")]
public class CalendarController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly CalendarMate.Data.CalendarDbContext _context;

    public CalendarController(CalendarMate.Data.CalendarDbContext context, ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<IResult> Get()
    {
        var events = await _context.Events.ToListAsync();
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

        await _context.AddAsync(newEvent);
        await _context.SaveChangesAsync();

        return Results.Created($"/calendar/{newEvent.EventId}", _event);
    }

    [HttpDelete]
    public async Task<IResult> Delete(int id)
    {
        if (await _context.Events.FindAsync(id) is CalendarMate.Models.Event _event)
        {
            _context.Events.Remove(_event);
            await _context.SaveChangesAsync();
            return TypedResults.Ok(_event);
        }

        return TypedResults.NotFound();
    }
}