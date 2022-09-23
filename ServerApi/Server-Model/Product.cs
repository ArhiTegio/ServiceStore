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
    public class ProductModel
    {
        private readonly ApplicationDatabaseContext _db;
        private readonly ILogger<EntitysModel> _logger;
        public ProductModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this._db = db;
            this._logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Product>)this._db.Product;
        }

        public async Task<IEnumerable<Product>> GetOneByIdAsync(int id)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");

            return (IEnumerable<Product>)this._db.Product.Where(x => x.Id == id);
        }

        public async Task AddAsync(string name, double price)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var product = new Product();
                product.Name = name;
                product.Price = (decimal)price;
                if (_db.Product.FirstOrDefault() is null)
                    product.Id = 1;
                else
                    product.Id = _db.Product.Max(x => x.Id) + 1;

                _db.Product.Add(product);
                await _db.SaveChangesEntitisAsync();
            }
            catch (Exception ex)
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
                var product = _db.Product.Where(x => x.Name.Contains(name)).FirstOrDefault();

                if (product != null)
                {
                    _db.Product.Remove(product);
                    await _db.SaveChangesEntitisAsync();
                }
            }
            catch (Exception ex)
            {
                this._logger.LogWarning($"{DateTime.Now} - Данные не записались в БД - {ex.Message} - {ex.StackTrace}");
            }
        }

        public async Task ChangePriceById(int id, double price)
        {
            while (!await this._db.Database.CanConnectAsync())
                this._logger.LogInformation($"{DateTime.Now} - Ожидание БД");
            try
            {
                var product = _db.Product.Where(x => x.Id == id).FirstOrDefault();

                if (product != null)
                {
                    product.Price = (decimal)price;
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
