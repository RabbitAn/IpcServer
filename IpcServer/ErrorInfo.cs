namespace IpcServer;

public class ErrorInfo
{
   public Level Level { get; set; }
   public DateTime time { get; set; }
   public string description { get; set; }
}

public enum Level
{
   error ,
   warning,
   info
}