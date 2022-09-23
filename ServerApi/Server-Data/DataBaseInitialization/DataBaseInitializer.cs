using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace Server_Data.DataBaseInitialization
{
    public class DataBaseInitializer
    {
        public Context.ApplicationDatabaseContext db;

        public DataBaseInitializer(Context.ApplicationDatabaseContext db)
        {
            this.db = db;
        }

        public void Init() => InitAsync().Wait();

        public async Task InitAsync()
        {
            var _db = db.Database;
            await _db.MigrateAsync().ConfigureAwait(false);
        }
    }
}
