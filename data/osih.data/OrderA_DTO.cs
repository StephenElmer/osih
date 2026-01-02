using System;
using System.Collections.Generic;
using System.Text;

namespace osih.data
{
    internal class OrderA_DTO
    {
        public string? orderID { get; set; }
        public string? customer { get; set; }
        public string? orderDate { get; set; }
        public double? totalAmount { get; set; }
        public string? status { get; set; }
    }
}
