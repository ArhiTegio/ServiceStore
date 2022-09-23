using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace WebApi.Logging
{
    public static class FileLoggerExtensions
    {
        public static ILoggerFactory AddFile(this ILoggerFactory factory,
                                        string pathDirectory)
        {
            var path = Path.Combine(pathDirectory, DateTime.Now.ToString("yyyy-MM-dd") + ".log");
            if (!File.Exists(path))
                File.Create(path).Close();
            factory.AddProvider(new FileLoggerProvider(path));
            return factory;
        }
    }
}
