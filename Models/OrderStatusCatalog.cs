using System;
using System.ComponentModel.DataAnnotations;

namespace LiquefiedPetroleumGas.Models
{
    public class OrderStatusCatalog
    {
        public string Id { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}