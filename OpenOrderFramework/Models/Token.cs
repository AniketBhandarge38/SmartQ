using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenOrderFramework.Models
{
    public class Token
    {
        public int TokenID { get; set; }
        public int OrderId { get; set; }
        public string TokenSlot { get; set; }
        public System.DateTime TokenDT { get; set; }
    }
}