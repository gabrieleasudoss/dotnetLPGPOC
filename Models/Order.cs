using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquefiedPetroleumGas.Models
{
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public string CustomerId { get; set; }
        public string PaymentTypeId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryAgent { get; set; }
        [Required]
        public string DeliveryAddress { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Users Users { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
