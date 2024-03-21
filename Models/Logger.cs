using System.Collections.Concurrent;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

public class Logger
{
    private static Logger? instance = null;
    private static readonly object padlock = new object();
    private ConcurrentQueue<string> logQueue = new ConcurrentQueue<string>();
    private string logFilePath = "log.txt";

    public static Logger Instance
    {
        get
        {
            lock (padlock)
            {
                if (instance == null)
                {
                    instance = new Logger();

                }
                return instance;
            }
        }
    }
    Logger()
    {
        if (File.Exists(logFilePath))
        {
            File.Delete(logFilePath);
        }
    }

    public async Task Log(string message,
    [CallerMemberName] string memberName = "",
    [CallerLineNumber] int lineNumber = 0,
    [CallerFilePath] string filePath = "")
    {
        var className = Path.GetFileNameWithoutExtension(filePath);
        var logMessage = $"[{className}::{memberName}::{lineNumber}]: {message}";
        logQueue.Enqueue(logMessage);
        await WriteLog();
    }



    private async Task WriteLog()
    {
        while (!logQueue.IsEmpty)
        {
            if (logQueue.TryDequeue(out string logEntry))
            {
                using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
                {
                    await streamWriter.WriteLineAsync(logEntry);
                }
            }
        }
    }
}
