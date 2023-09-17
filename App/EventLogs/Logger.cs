using GroupProject.App.BankManagement.User;
using GroupProject.App.ConsoleHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationUtility;

namespace GroupProject.App.EventLogs
{
  public class Logger
  {
    private readonly string _LOGS;
    private readonly string _FOLDER;
    private readonly string _PATH;

    public Logger(string logs = "Logs.txt", string folder = "CustomFiles\\EventLogs")
    {
      _LOGS = logs;
      _FOLDER = folder;
      _PATH = Path.Combine(_FOLDER, _LOGS);

      if (!Directory.Exists(_FOLDER))
      {
        try
        {
          Directory.CreateDirectory(_FOLDER);
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
      if (!File.Exists(_PATH))
      {
        try
        {
          File.Create(_PATH).Close();
        }
        catch (Exception ex)
        {
          ExceptionHelper.ExceptionDetails(ex);
        }
      }
    }
    public void LogAttemptedEvent(string attemptededEvent, string message)
    {
      Log($"{attemptededEvent.ToUpper()} ATTEMPT", message);
    }
    public void LogFailedEvent(string failedEvent, string message)
    {
      Log($"{failedEvent.ToUpper()} - FAILED", message);
    }
    public void LogSuccessfulEvent(string successefulEvent, string message)
    {
      Log($"{successefulEvent.ToUpper()} - SUCCESSFUL", message);
    }
    public void LogTransferEvent(string transferEvent, string message)
    {
      Log($"{transferEvent.ToUpper()} ATTEMPT", message);
    }
    public void LogInfo(string message)
    {
      Log("INFO", message);
    }
    public void LogWarning(string message)
    {
      Log("WARNING", message);
    }
    public void LogError(string message, Exception ex = null)
    {
      Log("ERROR", message, ex);
    }
    private void Log(string level, string message, Exception exception = null)
    {
      try
      {
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] - {message}";
        if (exception != null)
        {
          logMessage += Environment.NewLine + exception.ToString();
        }
        File.AppendAllText(_PATH, logMessage + Environment.NewLine);
      }
      catch (Exception ex)
      {
        Console.WriteLine("Failed to log message...");
        ExceptionHelper.ExceptionDetails(ex);

      }
    }
  }
}
