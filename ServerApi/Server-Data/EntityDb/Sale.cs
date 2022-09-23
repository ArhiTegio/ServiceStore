using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Server_Data.EntityDb
{
    public class Sale: Base.Modification
    {
        public Sale()
        {
            SalesPointId = new HashSet<SalesPoint>();
            SalesDataId = new HashSet<SalesData>();
        }
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }
        [Column(TypeName = "time")]
        public TimeSpan Time { get; set; }
        public ICollection<SalesPoint> SalesPointId { get; set; }
        public int? BuyerId { get; set; }
        public ICollection<SalesData> SalesDataId { get; set; }
        public int TotalAmount { get; set; } 
        public Bayer? Bayer { get; set; }

    }
}
