using GroupProject.BankDatabase.EventLogs.Events;
using GroupProject.BankDatabase.JsonConverters;
using Newtonsoft.Json;
using System.Threading;
using ValidationUtility;

namespace GroupProject.BankDatabase.EventLogs
{
  public class Logger
  {
    private readonly string _USERLOGS;
    private readonly string _FOLDERUSER;
    private readonly string _DATABASELOGS;
    private readonly string _FOLDERDATABASE;
    private readonly string _PATHUSER;
    private readonly string _PATHDATABASE;
    private readonly int _saveInterval = 15 * 60 * 1000; // 15 minutes in milliseconds

    private CancellationTokenSource _cancellationTokenSource;

    private EventLog _databaseLogs;
    private UserLogs _userLogs { get; set; }


    public Logger()
    {
      _userLogs = new UserLogs();
      _databaseLogs = new ConnectionLog("LOGGER", "Created the connection between the logger and the database");
      _USERLOGS = "UserLogs.json";
      _DATABASELOGS = "DatabaseLogs.json";

      _FOLDERUSER = "CustomFiles\\EventLogs\\UserLogs";
      _FOLDERDATABASE = "CustomFiles\\EventLogs\\DatabaseLogs";

      _PATHUSER = Path.Combine(_FOLDERUSER, _USERLOGS);
      _PATHDATABASE = Path.Combine(_FOLDERDATABASE, _DATABASELOGS);

      _cancellationTokenSource = new CancellationTokenSource();

      Task.Run(() => PeriodicLogSaveAsync(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

      InitializeLogs();
    }

    private void InitializeLogs()
    {
      if (Directory.Exists(_FOLDERDATABASE))
      {
        try
        {
          if (File.Exists(_PATHDATABASE))
          {
            using (StreamReader sr = new StreamReader(_PATHDATABASE))
            {
              var jsonLogs = sr.ReadToEnd();

              _databaseLogs = JsonConvert.DeserializeObject<EventLog>(jsonLogs, new JsonSerializerSettings
              {
                TypeNameHandling = TypeNameHandling.Objects,
                Converters = { new CustomLogConverter() }
              });
            }
          }
          else
          {
            _databaseLogs = new ConnectionLog("DATABASE", "Logger Created Database Logs");
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine("Failed to log user logs");
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
      if (Directory.Exists(_FOLDERUSER))
      {
        try
        {
          Directory.CreateDirectory(_FOLDERUSER);
          if (File.Exists(_PATHUSER))
          {
            using (StreamReader sr = new StreamReader(_PATHUSER))
            {
              var jsonLogs = sr.ReadToEnd();
              _userLogs = JsonConvert.DeserializeObject<UserLogs>(jsonLogs, new JsonSerializerSettings
              {
                TypeNameHandling = TypeNameHandling.Objects,
                Converters = { new CustomLogConverter() }
              });
            }
          }
          else
          {
            _userLogs = new UserLogs();
          }
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
      else
      {
        Directory.CreateDirectory(_FOLDERUSER);
        File.CreateText(_PATHUSER).Close();
        _userLogs = new UserLogs();
        _databaseLogs = new ConnectionLog("DATABASE", "Database created its first log");
        Thread.Sleep(10);
      }

    }
    public void Log(EventLog log)
    {
      try
      {
        if (_userLogs == null)
        {
          _userLogs = new UserLogs();
        }
        _userLogs.AddUserLog(log.Username, log);
        WriteUserLogsToFile();
      }
      catch (Exception exception)
      {
        Console.WriteLine("Failed to log message...");
        ExceptionHelper.ExceptionDetails(exception);
        Console.ReadKey();

      }
    }

    internal List<EventLog> GetUserLogs(string username)
    {
      try
      {

        using (StreamReader sr = new(_PATHUSER))
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

    private void WriteUserLogsToFile()
    {
      var jsonSettings = new JsonSerializerSettings
      {
        TypeNameHandling = TypeNameHandling.Objects,
        Converters = { new CustomLogConverter() }
      };
      var updatedLogsJson = JsonConvert.SerializeObject(_userLogs, Formatting.Indented, jsonSettings);

      File.WriteAllText(_PATHUSER, updatedLogsJson);
    }


    private async Task PeriodicLogSaveAsync(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        await Task.Delay(_saveInterval, cancellationToken);

        // Save logs to the file periodically
        WriteUserLogsToFile();
      }
    }
    public void Dispose()
    {
      // Stop the background task when the logger is disposed
      _cancellationTokenSource.Cancel();
      _cancellationTokenSource.Dispose();
    }


  }
}