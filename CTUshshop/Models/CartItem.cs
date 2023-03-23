using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTUshshop.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public List<ProductImg> ProductImg { get; set; }
        public string UserName { get; set; }
        public int Quantity { get; set; }

        public double SumPrice()
        {
            return (double)(Quantity * Product.Price);
        }
    }
}
