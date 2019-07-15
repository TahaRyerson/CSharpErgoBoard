using System;
using System.Collections;
using System.Collections.Generic;
using System.Management;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;


namespace CSharpErgoBoard
{
    /// <summary>
    /// Holds logs stored in the queue.
    /// </summary>
    class LogData
    {
        public LogData() { }
        private String message;
        private String time;
        private String date;
        private String threadName;
        private String memberName;
        private String fileName;
        private String lineNumber;

        public string Message { get => message; set => message = value; }
        public string Date { get => date; set => date = value; }
        public string ThreadName { get => threadName; set => threadName = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string LineNumber { get => lineNumber; set => lineNumber = value; }
        public string Time { get => time; set => time = value; }
        public string MemberName { get => memberName; set => memberName = value; }
    }

    /// <summary>
    /// A class used for creating logs and storing them in the correct files and folders. 
    /// </summary>
    class Logging
    {
        private static String m_logFormat = "%D (%T), \"%F\" <%L> : %M";
        private static Queue<LogData> m_output = new Queue<LogData>();
        private static Mutex m_outputLock = new Mutex();
        private static Thread m_thread = new Thread(LoggingThreadFunction);
        private static Boolean m_instance = false;
        private static String m_directory = "Logs.log";

        public static string Directory { get => m_directory; set => m_directory = value; }
        public static string LogFormat { get => m_logFormat; set => m_logFormat = value; }

        /// <summary>
        /// Default Constructor. Starts the threading process and creates out instance. 
        /// </summary>
        public Logging()
        {
            m_instance = true;
            m_thread.Start();
        }

        /// <summary>
        /// Adds a log to the log queue
        /// </summary>
        /// <param name="message"> This is the message you want saved to log </param>
        /// <param name="memberName"> Using macros finds the member name that made the log</param>
        /// <param name="filePath"> Using macros finds the current file name that the log was made on</param>
        /// <param name="lineNumber"> Using macros finds the line number that the log was made on</param>
        public void Log(String message,
        [System.Runtime.CompilerServices.CallerMemberName] String memberName = "",
        [System.Runtime.CompilerServices.CallerMemberName] String filePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            LogData newLog = new LogData
            {
                Message = message,
                Date = DateTime.Today.ToShortDateString(),
                //ThreadName = Thread.Get,
                FileName = filePath,
                LineNumber = lineNumber.ToString(),
                Time = DateTime.Now.ToString("h:mm:ss tt"),
                MemberName = memberName
            };
            m_outputLock.WaitOne();
            m_output.Enqueue(newLog);
            m_outputLock.ReleaseMutex();
        }

        /// <summary>
        /// A function used by a thread created from the constructor. The function would create logs messaged based on the log format and save them in the corresponding file. 
        /// </summary>
        private static void LoggingThreadFunction()
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Taha Abbasi-Hashemi\source\repos\CSharpErgoBoard\CSharpErgoBoard\Logs.log");

            while (m_instance)
            {
                // No members found.
                if (m_output.Count() == 0)
                {
                    continue;
                }
                LogData writeLog = m_output.Dequeue();

                String message = "";
                for (int i = 0; i < m_logFormat.Count(); i++)
                {
                    if (m_logFormat.ElementAt(i) == '%')
                    {
                        Char parameter = m_logFormat.ElementAt(i + 1);
                        if (parameter == 'D')
                        {
                            message += writeLog.Date;
                        }
                        else if (parameter == 'T')
                        {
                            message += writeLog.Time;
                        }
                        else if (parameter == 't')
                        {
                            message += writeLog.ThreadName;
                        }
                        else if (parameter == 'm')
                        {
                            message += writeLog.MemberName;
                        }
                        else if (parameter == 'F')
                        {
                            message += writeLog.FileName;
                        }
                        else if (parameter == 'L')
                        {
                            message += writeLog.LineNumber;
                        }
                        else if (parameter == 'M')
                        {
                            message += writeLog.Message;
                        }
                        else
                        {
                            message += '%';
                            message += parameter;
                        }
                        i++;
                    }
                    else
                    {
                        message += m_logFormat.ElementAt(i);
                    }
                }


                file.WriteLine(message);
            }
            file.Close();
        }

        /// <summary>
        /// Kills the entire logging process. 
        /// </summary>
        public void End()
        {
            m_instance = false;
            m_thread.Join();
        }
    }

}
