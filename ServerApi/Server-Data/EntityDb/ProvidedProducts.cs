using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_Data.EntityDb
{
    public class ProvidedProducts: Base.Modification
    {
        public ProvidedProducts()
        {
            SalesPointId = new HashSet<SalesPoint>();
        }

        public Product Product_ { get; set; }
        public int ProductId { get; set; }
        public ICollection<SalesPoint> SalesPointId { get; set; }
        public int ProductQuantity { get; set; }
    }
}
