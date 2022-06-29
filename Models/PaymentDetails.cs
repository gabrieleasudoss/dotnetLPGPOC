using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquefiedPetroleumGas.Models
{
    public class PaymentDetails
    {
        [Key]
        public string Id { get; set; }
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalCost { get; set; }
        [ForeignKey("PaymentId")]
        public virtual Payment Payment { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
