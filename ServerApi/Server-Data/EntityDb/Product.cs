using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_Data.EntityDb
{
    public class Product: Base.Modification
    {
        public Product()
        {
            ProvidedProducts_ = new HashSet<ProvidedProducts>();
            SalesData_ = new HashSet<SalesData>();
        }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public ICollection<ProvidedProducts> ProvidedProducts_ { get; set; }
        public ICollection<SalesData> SalesData_ { get; set; }
    }
}
