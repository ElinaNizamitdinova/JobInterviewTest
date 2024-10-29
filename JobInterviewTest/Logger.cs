using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobInterviewTest
{
    public class Logger
    {
        public static void Log(string logFilePath, string message)
        {

            if (!File.Exists(logFilePath))
            {
                using (FileStream fs = File.Create(logFilePath))
                {

                }

                using (StreamWriter logFile = new StreamWriter(logFilePath, true))
                {
                    logFile.WriteLine($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
                }
            }
        }
    }
}
