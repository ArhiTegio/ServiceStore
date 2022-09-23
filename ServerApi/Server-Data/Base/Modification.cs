using System;
using System.Collections.Generic;
using System.Text;

namespace Server_Data.Base
{
    public abstract class Modification: Identity
    {
        public DateTime CreateOn { get; set; }
        public DateTime UpdateOn { get; set; }
        public DateTime? DeleteOn { get; set; }
    }
}
