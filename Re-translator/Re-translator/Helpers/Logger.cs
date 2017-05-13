using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace Proxy
{
    public class telepasu
    {
        private static readonly string LogFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\telepasu-log\\";
        private static readonly AutoResetEvent EventSemaphore = new AutoResetEvent(true);
        static StreamWriter _file = null;

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
                StringBuilder sb = new StringBuilder();
                _file = new StreamWriter(LogFilePath + "tp-logger" + ".log", true);

                sb.AppendLine(date.ToString(CultureInfo.InvariantCulture));
                sb.AppendLine("###Необработанное исключение!");
                sb.AppendLine("Информация об ошибке: " + e.Message);
                sb.AppendLine("Stack trace: " + e.StackTrace);
                sb.AppendLine("Экземпляр класса исключения: " + e.InnerException);
                sb.AppendLine("Помощь: " + e.HelpLink);
                sb.AppendLine();
                _file.WriteLine(sb.ToString());
                Console.WriteLine(sb.ToString());
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _file.Close();
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
                _file = new StreamWriter(LogFilePath + "tp-logger" + ".log", true);
                _file.WriteLine(date.ToString(CultureInfo.InvariantCulture));
                _file.WriteLine(log);
                _file.WriteLine("");
            }
            catch (Exception)
            {
                // ignored
            }
            finally
            {
                _file.Close();
            }
            EventSemaphore.Set();
        }
    }
}