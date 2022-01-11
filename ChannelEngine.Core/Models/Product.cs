using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChannelEngine.Core.Models
{
    public class Product
    {
        public decimal Quantity { get; set; }
        public string MerchantProductNo { get; set; }
        public string Gtin { get; set; }
        public string Description { get; set; }
    }
}
