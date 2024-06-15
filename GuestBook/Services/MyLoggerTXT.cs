namespace GuestBook.Services
{
    public class MyLoggerTXT : IMyLogger
    {
        private readonly string _filePath;        

        public MyLoggerTXT(string filePath)
        {
            _filePath = filePath;            
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now} : {message}";
            File.AppendAllText(_filePath, logMessage + Environment.NewLine);
        }        
    }
}
