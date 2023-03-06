namespace CalendarMate.Services;

public interface IAuthManager
{
    IResult CheckSession(HttpRequest request, out int? userId);
}

public class AuthManager : IAuthManager
{
    private readonly ISessionStorage _session;

    public AuthManager(ISessionStorage session)
    {
        _session = session;
    }

    public IResult CheckSession(HttpRequest request, out int? userId)
    {
        userId = null;
        var ssid = request.Cookies["sessionId"];
        if (ssid == null) return Results.Unauthorized();
        if (!Guid.TryParse(ssid, out var guid)) return Results.BadRequest();
        if (!_session.TryGetUser(guid, out userId)) return Results.Unauthorized();

        return Results.Ok();
    }
}