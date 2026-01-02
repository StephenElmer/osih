using System;
using System.Collections.Generic;
using System.Text;

namespace osih.data
{
    internal class OrderB_DTO
    {
        public string? order_num { get; set; } 
        public string? client_name { get; set; }
        public string? date_placed { get; set; }
        public double? total { get; set; }
        public string? order_status { get; set; }
    }
}
