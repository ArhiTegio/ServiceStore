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
    public class SaleModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public SaleModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<Sale>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Sale>)this._db.Sale;
        }

        public async Task<IEnumerable<Sale>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Sale>)this._db.Sale.Where(x => x.Id == id);
        }

        public async Task<decimal> AddAsync(int total_amount, int? id_bayer, bool user)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            decimal id = -1;
            try
            {
                var sale = new Sale();

                sale.BuyerId = id_bayer;
                sale.Date = DateTime.Now;
                sale.Time = DateTime.Now.TimeOfDay;
                sale.TotalAmount = (int)total_amount;
                if (_db.Sale.FirstOrDefault() is null)
                    sale.Id = 1;
                else
                    sale.Id = _db.Sale.Max(x => x.Id) + 1;

                if (!user)
                    sale.BuyerId = null;

                id = sale.Id;
                _db.Sale.Add(sale);
                await _db.SaveChangesEntitisAsync();               
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
            return id;
        }

        public async Task DeleteAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var sale = _db.Sale.Where(x => x.Id == id).FirstOrDefault();

                if (sale != null)
                {
                    _db.Sale.Remove(sale);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task ChangeTotalAmountById(int id, int count)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var sales = _db.Sale.Where(x => x.Id == id).FirstOrDefault();

                if (sales != null)
                {
                    sales.TotalAmount = count;
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
