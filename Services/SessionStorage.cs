using CalendarMate.Models;
namespace CalendarMate.Services;

public interface ISessionStorage
{
    bool AddUser(ApplicationUser user, out Guid guid);
    bool RemoveUser(Guid guid);
    ApplicationUser GetUser(Guid guid);
    bool Exist(Guid guid);
}

public class SessionStorage : ISessionStorage
{
    private static readonly Dictionary<Guid, ApplicationUser> session = new Dictionary<Guid, ApplicationUser>();

    public bool AddUser(ApplicationUser user, out Guid guid)
    {
        var ssid = Guid.NewGuid();

        session[ssid] = user;
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

    public bool Exist(Guid guid)
    {
        Print();
        return session.ContainsKey(guid);
    }

    public ApplicationUser GetUser(Guid guid)
    {
        Print();
        return session[guid];
    }

    private void Print()
    {
        Console.WriteLine("Session List:");
        foreach (var kv in session)
        {
            Console.WriteLine($"SSID: {kv.Key.ToString()}, Username: {kv.Value.UserName}");
        }
    }
}