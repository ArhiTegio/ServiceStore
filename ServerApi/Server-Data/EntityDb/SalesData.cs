using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_Data.EntityDb
{
    public class SalesData: Base.Modification
    {
        public SalesData()
        {
        }

        public int ProductId { get; set; }
        public Product Product_ { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductIdAmount { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
    }
}
