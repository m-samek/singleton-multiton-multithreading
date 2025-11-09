namespace Singleton;

public class Logger
{
    private static Logger _instance;
    
    private Logger() { }
    
    public static Logger Instance()
    {
        if (_instance == null)
        {
            _instance = new Logger();
        }
        return _instance;
    }
    
    public void LoggActivity(string log)
    {
        Console.WriteLine($"Log: {log}");
    }
    
    static void Main(string[] args)
    {
        var logger = Logger.Instance();
        logger.LoggActivity("First log");
        var logger2 = Logger.Instance();
        logger2.LoggActivity("Second log");
        
        Console.WriteLine(object.ReferenceEquals(logger, logger2) ? "True" : "False");
    }
}