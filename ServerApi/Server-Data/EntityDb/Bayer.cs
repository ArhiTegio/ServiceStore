using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server_Data.EntityDb
{
    public class Bayer: Base.Modification
    {
        public Bayer()
        {
            SalesIds = new HashSet<Sale>();
        }

        [Column(TypeName = "varchar(256)")]
        public string Name { get; set; }
        public ICollection<Sale> SalesIds { get; set; }
    }
}
