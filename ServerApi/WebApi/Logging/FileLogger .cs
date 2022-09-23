using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace WebApi.Logging
{
    public class FileLogger : ILogger
    {
        private string filePath;
        private static object _lock = new object();
        private int _numberOfTries;
        private int _timeIntervalBetweenTries;

        public FileLogger(string path)
        {
            this.filePath = path;
            _numberOfTries = 20;
            _timeIntervalBetweenTries = 100;
        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            //return logLevel == LogLevel.Trace;
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter != null)
            {
                var txt = formatter(state, exception) + Environment.NewLine;
                var tries = 0;
                if (txt != LoggerBuffer.prevMessage)
                {
                    LoggerBuffer.prevMessage = new string(txt);
                    txt = DateTime.Now + " - " + txt;
                    while (true)
                    {
                        try
                        {
                            File.AppendAllText(filePath, txt);
                            break;
                        }
                        catch (IOException e)
                        {
                            if (!IsFileLocked(e))
                                Thread.Sleep(_timeIntervalBetweenTries);
                            else
                                ++tries;

                            if (++tries > _numberOfTries)
                                break;
                        }
                    }
                }

            }
        }

        private static bool IsFileLocked(IOException exception)
        {
            int errorCode = Marshal.GetHRForException(exception) & ((1 << 16) - 1);
            return errorCode == 32 || errorCode == 33;
        }
    }
}
