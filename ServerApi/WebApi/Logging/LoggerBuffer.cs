using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Logging
{
    public static class LoggerBuffer
    {
        public static string prevMessage { get; set; } = "";
    }
}
