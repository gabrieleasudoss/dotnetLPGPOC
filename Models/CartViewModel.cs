using System.Collections.Generic;

namespace LiquefiedPetroleumGas.Models
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
