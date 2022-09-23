using System;
using System.Configuration;

namespace Entity.Database
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public string ConnectionString { get
        {
                var t = ConfigurationManager.AppSettings;
                return ConfigurationManager.AppSettings[nameof(DatabaseConfig) + "_ConnectionString"];
        } }
        public string User { get => ConfigurationManager.AppSettings[nameof(DatabaseConfig) + "_User"]; }
        public string Password { get => ConfigurationManager.AppSettings[nameof(DatabaseConfig) + "_Password"]; }
    }
}
