using GroupProject.App.BankManagement.User;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using System.IO;
using ValidationUtility;
using static System.Reflection.Metadata.BlobBuilder;

namespace GroupProject.BankDatabase.EventLogs
{
  public class Logger
  {
    private readonly string[] _LOGFILENAMES;
    private readonly string[] _BASEFOLDERS;


    public Logger()
    {
      _LOGFILENAMES = new string[]
      {
        "UserLogs.json",
        "DatabaseLogs.json"
      };
      _BASEFOLDERS = new string[]
      {
        "CustomFiles\\EventLogs\\UserLogs",
        "CustomFiles\\EventLogs\\DatabaseLogs"
      };

      InitializeLogs();
    }

    private void InitializeLogs()
    {
      foreach (string baseFolder in _BASEFOLDERS)
      {
        foreach (string logFileName in _LOGFILENAMES)
        {

          string logFilePath = Path.Combine(baseFolder, logFileName);

          string logDirectory = Path.GetDirectoryName(logFilePath);
          if (!Directory.Exists(logDirectory))
          {
            try
            {
              Directory.CreateDirectory(logDirectory);
            }
            catch (Exception ex)
            {
              ExceptionHelper.ExceptionDetails(ex);
            }
          }
          if (!File.Exists(logFilePath))
          {
            try
            {
              File.Create(logFilePath).Close();
            }
            catch (Exception ex)
            {
              ExceptionHelper.ExceptionDetails(ex);
            }
          }
        }
      }
    }

    public void LogEvent(string username, string message, EventCategory category, Exception ex = null)
    {
      Log(username, message, category, ex);
    }
    public void LogEvent(EventLog logEvent)
    {
      Log(logEvent.Username, logEvent.Message, logEvent.EventCategory, logEvent.Ex);
    }
    private void Log(string username, string message, EventCategory category, Exception ex = null)
    {
      try
      {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{category}] - {message}";
        if (ex != null)
        {
          logMessage += Environment.NewLine + ex.ToString();
        }
        File.AppendAllText(_PATH, logMessage + Environment.NewLine);
      }
      catch (Exception exception)
      {
        Console.WriteLine("Failed to log message...");
        ExceptionHelper.ExceptionDetails(exception);

      }
    }

    internal List<EventLog> GetUserLogs(string username)
    {
      try
      {
        string pathUserLogs = Path.Combine(_BASEFOLDERS[0], _LOGFILENAMES[0]);
        using (StreamReader sr = new(pathUserLogs))
        {
          var jsonLogs = sr.ReadToEnd();
          List<EventLog>? userLogs = JsonConvert.DeserializeObject<List<EventLog>>(jsonLogs, new JsonSerializerSettings
          {
            TypeNameHandling = TypeNameHandling.Objects,
            Converters = { new CustomLogConverter() }
          });

          List<EventLog> foundLogs = new();
          foreach (var log in userLogs)
          {
            if (log.Username == username)
            {
              foundLogs.Add(log);
            }
          }

          return foundLogs;

        }
      }
      catch (Exception ex)
      {
        ExceptionHelper.ExceptionDetails(ex);
        Console.ReadKey();
        return null;
      }
    }

  }




















//public void LogAttemptedEvent(string attemptededEvent, string message)
//{
//  Log($"{attemptededEvent.ToUpper()} ATTEMPT", message);
//}
//public void LogFailedEvent(string failedEvent, string message)
//{
//  Log($"{failedEvent.ToUpper()} - FAILED", message);
//}
//public void LogSuccessfulEvent(string successefulEvent, string message)
//{
//  Log($"{successefulEvent.ToUpper()} - SUCCESSFUL", message);
//}
//public void LogTransferEvent(string transferEvent, string message)
//{
//  Log($"{transferEvent.ToUpper()} ATTEMPT", message);
//}
//public void LogInfo(string message)
//{
//  Log("INFO", message);
//}
//public void LogWarning(string message)
//{
//  Log("WARNING", message);
//}
//public void LogError(string message, Exception ex = null)
//{
//  Log("ERROR", message, ex);
//}
