using System;
using System.IO;
using System.Threading;

namespace Proxy
{
    public class LogMonitor
    {
        public static string LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\telepasu-log\\";
        public static AutoResetEvent EventSemaphore = new AutoResetEvent(true);
        static StreamWriter file = null;

        public static void warn(string warning)
        {

        }
        public static void fatal(Exception e)
        {

        }
        public static void log(string log)
        {
            EventSemaphore.WaitOne();
            DateTime date = DateTime.Now;
            try
            {
                if (!Directory.Exists(LogFilePath))
                {
                    Directory.CreateDirectory(LogFilePath);
                }
                file = new StreamWriter(LogFilePath + "tp-logger" + ".log", true);
                file.WriteLine(date.ToString());
                file.WriteLine(log);
            }
            catch (Exception)
            {
            }
            finally
            {
                file.Close();
            }
            EventSemaphore.Set();
        }
    }
}