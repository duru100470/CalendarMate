using CalendarMate.Models;
namespace CalendarMate.Services;

public interface ISessionStorage
{
    bool AddUser(int userId, out Guid guid);
    bool RemoveUser(Guid guid);
    int GetUser(Guid guid);
    bool TryGetUser(Guid guid, out int? userId);
}

public class SessionStorage : ISessionStorage
{
    private static readonly Dictionary<Guid, Session> session = new Dictionary<Guid, Session>();
    private static readonly TimeSpan expires = new TimeSpan(1, 0, 0);

    public bool AddUser(int userId, out Guid guid)
    {
        var ssid = Guid.NewGuid();

        session[ssid] = new Session(userId, DateTime.Now.Add(expires));
        guid = ssid;
        Print();

        return true;
    }

    public bool RemoveUser(Guid guid)
    {
        var ret = session.Remove(guid);
        Print();

        return ret;
    }

    public int GetUser(Guid guid)
    {
        RemoveExpiredUser();
        Print();
        return session[guid].Id;
    }

    public bool TryGetUser(Guid guid, out int? userId)
    {
        RemoveExpiredUser();
        Print();
        var ret = session.TryGetValue(guid, out var user);
        userId = user?.Id;
        return ret;
    }

    private void Print()
    {
        Console.WriteLine("Session List:");
        foreach (var kv in session)
        {
            Console.WriteLine($"SSID: {kv.Key.ToString()}, UserId: {kv.Value.Id}, Expires: {kv.Value.Expires}");
        }
    }

    private void RemoveExpiredUser()
    {
        List<Guid> buffer = new List<Guid>();

        foreach (var kv in session)
        {
            if (kv.Value.IsExpired()) buffer.Add(kv.Key);
        }

        for (int i = buffer.Count - 1; i >= 0; i--)
        {
            session.Remove(buffer[i]);
        }
    }
}

public class Session
{
    public int Id;
    public DateTime Expires;

    public Session(int _Id, DateTime _Expires)
    {
        Id = _Id;
        Expires = _Expires;
    }

    public bool IsExpired()
    {
        return DateTime.Compare(DateTime.Now, Expires) > 0;
    }
}