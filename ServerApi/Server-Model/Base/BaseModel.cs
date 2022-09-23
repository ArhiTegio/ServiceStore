using Server_Data.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Server_Data.Context;

namespace Server_Model
{
    public class IModel
    {
        readonly ApplicationDatabaseContext _db;
        readonly ILogger _logger;
    }
}
