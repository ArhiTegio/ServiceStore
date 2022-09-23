using Server_Data.Context;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Server_Model.Base
{
    public class EntitysModel
    {
        public readonly ApplicationDatabaseContext db;
        public readonly ILogger<EntitysModel> logger;
        private BayerModel _bayer;
        public BayerModel bayer { get => _bayer; }

        private ProductModel _product;
        public ProductModel product { get => _product; }

        private ProvidedProductsModel _provided_products;
        public ProvidedProductsModel provided_products { get => _provided_products; }

        private SaleModel _sale;
        public SaleModel sale { get => _sale; }

        private SalesPointModel _salesPointModel;
        public SalesPointModel salesPointModel { get => _salesPointModel; }

        private SalesDataModel _salesDataModel;
        public SalesDataModel salesDataModel { get => _salesDataModel; }

        public EntitysModel(ApplicationDatabaseContext db, ILogger<EntitysModel> logger)
        {
            this.db = db; 
            this._bayer = new BayerModel(db, logger);
            this._product = new ProductModel(db, logger);
            this._provided_products = new ProvidedProductsModel(db, logger);
            this._sale = new SaleModel(db, logger);
            this._salesPointModel = new SalesPointModel(db, logger);
            this._salesDataModel = new SalesDataModel(db, logger);
        }
    }
}
