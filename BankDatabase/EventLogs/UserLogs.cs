using Newtonsoft.Json;

namespace GroupProject.BankDatabase.EventLogs
{
  public class UserLogs
  {
    [JsonProperty]
    public Dictionary<string, List<EventLog>> Users { get; set; }

    public UserLogs()
    {
      Users = new();
    }

    public void AddUserLog(string username, EventLog log)
    {
      if (!Users.ContainsKey(username))
      {
        Users[username] = new List<EventLog>();
      }

      Users[username].Add(log);
    }
    public List<EventLog> GetUserLogs(string username)
    {
      if (Users.ContainsKey(username))
      {
        return Users[username];
      }

      return new List<EventLog>();
    }

  }
}
