using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;


namespace Server_Data.Base
{
    public abstract class DbContextBase: DbContext
    {

        protected DbContextBase(DbContextOptions options) : base(options)
        {
            #if DEBUG
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
            #endif
        }

        public async Task<int> SaveChangesEntitisAsync()
        {
            try
            {
                int count = 0;
                var stats = new[] { EntityState.Added, EntityState.Modified, EntityState.Detached }; 
                var entities = ChangeTracker.Entries().Where(e => stats.Contains(e.State));
                foreach(var entity in entities)
                {

                    var date = DateTime.UtcNow;

                    if(entity.State == EntityState.Added)
                    {
                        if (entity.Properties.Where(x => x.Metadata.Name == "UpdateOn").Count() > 0)
                            entity.Property("UpdateOn").CurrentValue = date;
                        if (entity.Properties.Where(x => x.Metadata.Name == "CreateOn").Count() > 0)
                            entity.Property("CreateOn").CurrentValue = date;
                    }
                    else if(entity.State == EntityState.Modified)
                    {
                        if (entity.Properties.Where(x => x.Metadata.Name == "UpdateOn").Count() > 0)
                            entity.Property("UpdateOn").CurrentValue = date;
                    }
                    else if(entity.State == EntityState.Deleted)
                    {
                        if (entity.Properties.Where(x => x.Metadata.Name == "DeleteOn").Count() > 0)
                            entity.Property("DeleteOn").CurrentValue = date;
                        entity.State = EntityState.Unchanged;
                    }
                    count++;
                }
                await this.SaveChangesAsync();
                return count;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
                return 0;
            }
        }
    }
}
