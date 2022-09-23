using System;
using System.Collections.Generic;
using System.Text;

namespace Entity.Database
{
    public interface IDatabaseConfig
    {
        public string ConnectionString { get; }
        public string User { get; }
        public string Password { get; }
    }
}
