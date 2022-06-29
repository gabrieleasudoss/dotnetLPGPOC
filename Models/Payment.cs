using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquefiedPetroleumGas.Models
{
    public class Payment
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string PaymentTypeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, DataType(DataType.CreditCard), MinLength(16, ErrorMessage = "Minimum length is 16")]
        public string CardNumber { get; set; }
        [Required, DataType(DataType.Date)]
        public string ExpiryDate { get; set; }
        [Required, DataType(DataType.Password), MinLength(3, ErrorMessage = "Minimum length is 3")]
        public string Cvv { get; set; }
        [ForeignKey("PaymentTypeId")]
        public virtual PaymentType PaymentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
