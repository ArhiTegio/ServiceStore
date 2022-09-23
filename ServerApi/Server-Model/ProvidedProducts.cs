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
    public class ProvidedProductsModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public ProvidedProductsModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<ProvidedProducts>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<ProvidedProducts>)this._db.ProvidedProducts;
        }

        public async Task<IEnumerable<ProvidedProducts>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<ProvidedProducts>)this._db.ProvidedProducts.Where(x => x.Id == id);
        }

        public async Task<decimal> AddAsync(int id_product, int count)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            decimal id = -1;
            try
            {
                var product = new ProvidedProducts();
                var prod = _db.Product.Where(x => x.Id == id_product);
                if (prod != null)
                {
                    product.Product_ = prod.FirstOrDefault();
                    product.ProductQuantity = (int)count;
                    if (_db.ProvidedProducts.FirstOrDefault() is null)
                        product.Id = 1;
                    else
                        product.Id = _db.ProvidedProducts.Max(x => x.Id) + 1;

                    id = product.Id;
                    _db.ProvidedProducts.Add(product);
                    await _db.SaveChangesEntitisAsync();
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
                var product = _db.ProvidedProducts.Where(x => x.Id == id).FirstOrDefault();

                if (product != null)
                {
                    _db.ProvidedProducts.Remove(product);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task ChangePriceById(int id, int count)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var product = _db.ProvidedProducts.Where(x => x.Id == id).FirstOrDefault();

                if (product != null)
                {
                    product.ProductQuantity = count;
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
