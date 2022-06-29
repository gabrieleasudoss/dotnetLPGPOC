using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquefiedPetroleumGas.Models
{
    public class DeliveryDetails
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string ProductId { get; set; }
        [Required]
        public string DeliveryId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string UnitGasPrice { get; set; }
        [Required]
        public string GasPrice { get; set; }
    }
}
