namespace SingletonMultithreading;

public class Logger
{
    private static Logger? _instance;
    private static readonly object _lock = new object();
    
    private Logger() { }
    
    public static Logger Instance()
    {
        if (_instance == null)
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Logger();
                }
            }
        }
        return _instance;
    }

    public void LoggActivity(string log)
    {
        Console.WriteLine($"Log: {log}");
    }

    static void Main(string[] args)
    {
        var threads = new Thread[10];

        for (var i = 0; i < threads.Length; i++)
        {
            var threadId = i;
            threads[i] = new Thread(() =>
            {
                var logger = Logger.Instance();
                logger.LoggActivity($"Log from thread {threadId}");
            });
            threads[i].Start();
        }
        
        foreach (var t in threads)
        {
            t.Join();
        }

        Console.WriteLine("All threads finished.");
    }
}