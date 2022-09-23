using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Server_Data.Context;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace ServerApiTesting
{
    public class TestsApiBayer
    {
        private ApplicationDatabaseContext _db;
        private ApiWorker _api;

        public TestsApiBayer()
        {
            this._db = new ApplicationDatabaseContext(new DbContextOptionsBuilder<ApplicationDatabaseContext>().UseNpgsql("Server=127.0.0.1;Port=5432;User Id=postgres;Password=admin;Database=postgres;").Options);
            this._api = new ApiWorker("https://localhost:44372/");

            //_db.Bayer.RemoveRange(_db.Bayer);
            //_db.Product.RemoveRange(_db.Product);
            //_db.ProvidedProducts.RemoveRange(_db.ProvidedProducts);
            //_db.Sale.RemoveRange(_db.Sale);
            //_db.SalesData.RemoveRange(_db.SalesData);
            //_db.SalesPoint.RemoveRange(_db.SalesPoint);
            //_db.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void BayerAdd()
        {
            var arr = new List<string> { "Петров_test", "Сидоров_test", "Иванов1_test", "Иванов2_test", "Иванов3_test" };

            foreach (var e in arr)
                _api.Put("bayer/add?name=" + e, new byte[0]);

            var bayersAsync = _db.Bayer.ToListAsync();
            bayersAsync.Wait();
            var bayers = bayersAsync.Result;

            var count = 0;
            foreach (var bayer in bayers)
            {
                if (arr.Contains(bayer.Name))
                    count++;
            }

            Assert.IsTrue(arr.Count == count, "Не пройден тест " + System.Reflection.MethodBase.GetCurrentMethod().Name);


            foreach (var bayer in _db.Bayer)
            {
                if (arr.Contains(bayer.Name))
                {
                    _db.Bayer.Remove(bayer);
                }
            }

            _db.SaveChanges();
        }

        [Test]
        public void BayerDeleteById()
        {
            var arr = new List<string> { "Петров_test", "Сидоров_test" };

            foreach (var e in arr)
                _api.Put("bayer/add?name=" + e, new byte[0]);


            _api.Delete("bayer/delete?name=" + arr.First());

            var bayersAsync = _db.Bayer.ToListAsync();
            bayersAsync.Wait();
            var bayers = bayersAsync.Result;

            var count = 0;
            var isFoundName = false;
            foreach (var bayer in bayers)
            {
                if (arr.Contains(bayer.Name))
                {
                    count++;
                    isFoundName = arr.Last() == bayer.Name;
                }
            }

            Assert.IsTrue(1 == count && isFoundName, "Не пройден тест " + System.Reflection.MethodBase.GetCurrentMethod().Name);


            foreach (var bayer in _db.Bayer)
            {
                if (arr.Contains(bayer.Name))
                {
                    _db.Bayer.Remove(bayer);
                }
            }

            _db.SaveChanges();
        }

        class Bayer
        {
            public int Id {get;set;}
            public string Name { get; set; }
            public DateTime CreateOn { get; set; }
            public DateTime UpdateOn { get; set; }
            public DateTime? DeleteOn { get; set; }

        }


        [Test]
        public void BayerGetData()
        {
            var arr = new List<string> { "Петров_test", "Сидоров_test" };
            foreach (var e in arr)
                _api.Put("bayer/add?name=" + e, new byte[0]);

            var bayersAsync = _db.Bayer.ToListAsync();
            bayersAsync.Wait();
            var bayers = bayersAsync.Result;
            var pos = 0;
            foreach(var e in bayers)
            {
                if (arr.Last() == e.Name)
                    pos = e.Id;
            }

            var json = _api.Get("bayer/" + pos);
            var json_2 = JsonConvert.DeserializeObject<Bayer[]>(_api.Get("bayer"));
            Assert.IsTrue(json.Contains(arr.Last()), "Не пройден тест " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            Assert.IsTrue(arr.Count == json_2.Length, "Не пройден тест " + System.Reflection.MethodBase.GetCurrentMethod().Name);

            foreach (var bayer in _db.Bayer)
            {
                if (arr.Contains(bayer.Name))
                {
                    _db.Bayer.Remove(bayer);
                }
            }

            _db.SaveChanges();
        }
    }
}