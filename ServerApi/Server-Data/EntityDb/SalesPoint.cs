using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_Data.EntityDb
{
    public class SalesPoint: Base.Modification
    {
        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        public int ProvidedProductsId { get; set; }
        public ProvidedProducts ProvidedProducts_ { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
