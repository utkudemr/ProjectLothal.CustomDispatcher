namespace ProjectLothal.CustomDispatcher.Api.Decorators
{
    
    public class AuditLogAttribute : BaseAttribute
    {
        public AuditLogAttribute()
        {
        }
    }



    public class LogBusinessService: ILoggerService
    { 
        public LogBusinessService() { }

        public void LogWarning(string message) {
            Console.WriteLine(message);
        }
    }

    public interface ILoggerService
    {
        void LogWarning(string message);
    }


}
