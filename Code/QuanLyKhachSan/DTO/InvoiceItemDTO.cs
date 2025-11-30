using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class InvoiceItemDTO
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public string Unit {get; set;}
        public decimal Price { get; set; }
        public decimal Total => Quantity * Price;
    }
}
