using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server_Data.Context;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace ServerApiTesting
{
    public class DBClear
    {
        private ApplicationDatabaseContext _db;
        private ApiWorker _api;
        private Random _rand;

        public DBClear()
        {
            this._db = new ApplicationDatabaseContext(new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseNpgsql("Server=127.0.0.1;Port=5432;User Id=postgres;Password=admin;Database=postgres;").Options);
            this._api = new ApiWorker("https://localhost:44372/");
            this._rand = new Random();
        }

        [Test]
        public void ClearData()
        {
            _db.Bayer.RemoveRange(_db.Bayer);
            _db.Product.RemoveRange(_db.Product);
            _db.ProvidedProducts.RemoveRange(_db.ProvidedProducts);
            _db.Sale.RemoveRange(_db.Sale);
            _db.SalesData.RemoveRange(_db.SalesData);
            _db.SalesPoint.RemoveRange(_db.SalesPoint);
            _db.SaveChanges();
        }
    }
}
