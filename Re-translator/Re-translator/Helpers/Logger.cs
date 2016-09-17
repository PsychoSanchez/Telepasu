using System;
using System.IO;
using System.Threading;

namespace Proxy
{
    public class telepasu
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
        public static void exc(Exception e)
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
                file.WriteLine("###Необработанное исключение!");
                file.WriteLine("Информация об ошибке: " + e.Message);
                file.WriteLine("Stack trace: " + e.StackTrace);
                file.WriteLine("Экземпляр класса исключения: " + e.InnerException);
                file.WriteLine("Помощь: " + e.HelpLink);
                file.WriteLine();
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
        public static void log(string log)
        {
            Console.WriteLine(log);
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
                file.WriteLine("");
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