using Server_Data.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Server_Model.Base;
using System.Threading.Tasks;
using System.Linq;
using Server_Data.EntityDb;

namespace Server_Model
{
    public class BayerModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public BayerModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<Bayer>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Bayer>)this._db.Bayer;
        }

        public async Task<IEnumerable<Bayer>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Bayer>)this._db.Bayer.Where(x=> x.Id == id);
        }

        public async Task AddAsync(string name)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var bayer = new Bayer();
                bayer.Name = name;
                if (_db.Bayer.FirstOrDefault() is null)
                    bayer.Id = 1;
                else
                    bayer.Id = _db.Bayer.Max(x => x.Id) + 1;

                _db.Bayer.Add(bayer);
                await _db.SaveChangesEntitisAsync();
            }
            catch(Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task DeleteAsync(string name)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var bayer = _db.Bayer.Where(x => x.Name.Contains(name)).FirstOrDefault();

                if (bayer != null)
                {
                    _db.Bayer.Remove(bayer);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }
    }
}
