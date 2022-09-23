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
    public class SalesDataModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public SalesDataModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<SalesData>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<SalesData>)this._db.SalesData;
        }

        public async Task<IEnumerable<SalesData>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<SalesData>)this._db.SalesData.Where(x => x.Id == id);
        }

        public async Task<decimal> AddAsync(int products_id, int sale_id, int productQuantity, int productIdAmount)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            decimal id = -1;
            try
            {
                if (products_id > 0 && sale_id > 0)
                {
                    Sale sale = _db.Sale.Where(x => x.Id == sale_id).FirstOrDefault();
                    Product products = _db.Product.Where(x => x.Id == products_id).FirstOrDefault();

                    if (sale != null && products != null)
                    {
                        var saleData = new SalesData();
                        saleData.ProductIdAmount = productIdAmount;
                        saleData.ProductQuantity = productQuantity;
                        saleData.Sale = sale;
                        saleData.Product_ = products;
                        saleData.ProductId = products_id;
                        saleData.SaleId = sale_id;

                        if (_db.SalesData.FirstOrDefault() is null)
                            saleData.Id = 1;
                        else
                            saleData.Id = _db.SalesData.Max(x => x.Id) + 1;
                        id = saleData.Id;
                        _db.SalesData.Add(saleData);
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
                var sale = _db.SalesData.Where(x => x.Id == id).FirstOrDefault();

                if (sale != null)
                {
                    _db.SalesData.Remove(sale);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task ChangeById(int id, int? productQuantity, int? productIdAmount)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var sales = _db.SalesData.Where(x => x.Id == id).FirstOrDefault();

                if (sales != null)
                {
                    var pos = 0;
                    if (productQuantity != null)
                    {
                        sales.ProductQuantity = productQuantity.Value;
                        pos++;
                    }
                    if (productIdAmount != null)
                    {
                        sales.ProductIdAmount = productIdAmount.Value;
                        pos++;
                    }
                    if(pos > 0)
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
