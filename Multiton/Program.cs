namespace DatabaseConnectionManager;
    
public class DatabaseConnectionManager
{
    private static readonly Dictionary<string, DatabaseConnectionManager> _instances = new Dictionary<string, DatabaseConnectionManager>();
    private static readonly object _lock = new object();

    public bool IsConnected { get; private set; }
    public string Database { get; private set; }
    
    private DatabaseConnectionManager(string database)
    {
        Database = database;
        IsConnected = false;
    }
    
    public static DatabaseConnectionManager GetConnection(string database)
    {
        if (!_instances.ContainsKey(database))
        {
            lock (_lock)
            {
                if (!_instances.ContainsKey(database))
                {
                    _instances[database] = new DatabaseConnectionManager(database);
                }
            }
        }
        return _instances[database];
    }
    
    public void OpenConnection()
    {
        if (!IsConnected)
        {
            Console.WriteLine($"Connection with db: {Database} open.");
            IsConnected = true;
        }
        else
        {
            Console.WriteLine($"Connection with db: {Database} is already opened.");
        }
    }
    
    public void CloseConnection()
    {
        if (IsConnected)
        {
            Console.WriteLine($"Connection with db: {Database} closed.");
            IsConnected = false;
        }
        else
        {
            Console.WriteLine($"Connection with db: {Database} is already closed.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var conn1 = DatabaseConnectionManager.GetConnection("DB_A");
        conn1.OpenConnection();

        var conn2 = DatabaseConnectionManager.GetConnection("DB_B");
        conn2.OpenConnection();

        var conn3 = DatabaseConnectionManager.GetConnection("DB_A");
        Console.WriteLine(Object.ReferenceEquals(conn1, conn3)
            ? "conn1 and conn3 is the same instance"
            : "conn1 and conn3 are different instances");

        conn3.CloseConnection();
        conn2.CloseConnection();
        
        Console.WriteLine("---Multithreading check.---");
        
        Thread[] threads = new Thread[10];
        for (int i = 0; i < threads.Length; i++)
        {
            int threadNum = i;
            threads[i] = new Thread(() =>
            {
                string dbName = (threadNum % 2 == 0) ? "DB_A" : "DB_B";
                var connection = DatabaseConnectionManager.GetConnection(dbName);
                connection.OpenConnection();
                Console.WriteLine($"Thread {threadNum} serves db {dbName}");
                connection.CloseConnection();
            });
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        Console.WriteLine("Threads finished.");
    }
}
