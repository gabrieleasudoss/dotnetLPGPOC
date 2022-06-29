using System;
using System.Collections.Generic;

namespace LiquefiedPetroleumGas.Models
{
    public class OrderViewModel
    {
        public List<Order> Orders { get; set; }
        public List<Users> Customers { get; set; }
        public List<Users> Agent { get; set; }
        public List<OrderStatusCatalog> OrderStatusCatalogs { get; set; }
        public string AgentName { get; set; }
        public string Status { get; set; }
        public string Search { get; set; }
        public string Id { get; set; }
    }
}
