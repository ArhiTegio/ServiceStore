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
    public class SalesPointModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public SalesPointModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<SalesPoint>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<SalesPoint>)this._db.SalesPoint;
        }

        public async Task<IEnumerable<SalesPoint>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<SalesPoint>)this._db.SalesPoint.Where(x => x.Id == id);
        }

        public async Task<decimal> AddAsync(string name, int provided_products_id, int sale_id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            decimal id = -1;
            try
            {
                //var bayer = id_bayer is null? null: _db.Bayer.Where(x => x.Id == id_bayer).FirstOrDefault();
                if(provided_products_id > 0 && sale_id > 0)
                {
                    Sale sale = _db.Sale.Where(x => x.Id == sale_id).FirstOrDefault();
                    ProvidedProducts providedProducts = _db.ProvidedProducts.Where(x => x.Id == provided_products_id).FirstOrDefault();

                    if(sale != null && providedProducts != null)
                    {
                        var salePoint = new SalesPoint();
                        salePoint.Name = name;
                        salePoint.ProvidedProductsId = provided_products_id;
                        salePoint.SaleId = sale_id;
                        salePoint.Sale = sale;
                        salePoint.ProvidedProducts_ = providedProducts;

                        if (_db.SalesPoint.FirstOrDefault() is null)
                            salePoint.Id = 1;
                        else
                            salePoint.Id = _db.SalesPoint.Max(x => x.Id) + 1;
                        id = salePoint.Id;
                        _db.SalesPoint.Add(salePoint);
                        await _db.SaveChangesEntitisAsync();
                    }
                }
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
                var sale = _db.SalesPoint.Where(x => x.Id == id).FirstOrDefault();

                if (sale != null)
                {
                    _db.SalesPoint.Remove(sale);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task ChangeNameById(int id, string name)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var sales = _db.SalesPoint.Where(x => x.Id == id).FirstOrDefault();

                if (sales != null)
                {
                    sales.Name = name;
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
