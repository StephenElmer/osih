using System.Text.Json;
using osih.model;

namespace osih.data
{
    public class DataAccess
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public DataAccess() 
        {
            string systemAJson = AppContext.BaseDirectory + "system_a_orders.json";
            string systemBCsv = AppContext.BaseDirectory + "system_b_orders.csv";

            ReadSystemA(systemAJson);
            ReadSystemB(systemBCsv);
        }





        public void ReadSystemA(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            var myData = JsonSerializer.Deserialize<List<OrderA_DTO>>(jsonString);

            if (myData != null)
            {
                foreach (var item in myData)
                {
                    this.Orders.Add(
                        new Order
                        {
                            OrderId = item.orderID,
                            SourceSystem = "SystemA",
                            CustomerName = item.customer,
                            OrderDate = DateTime.Parse(item.orderDate),
                            TotalAmount = item.totalAmount,
                            Status = NormalizeStatus("SystemA", item.status)
                        }
                    );
                }
            }
        }


        public void ReadSystemB(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                using (var csv = new CsvHelper.CsvReader(reader, System.Globalization.CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<OrderB_DTO>();
                    foreach (var item in records)
                    {
                        this.Orders.Add(
                            new Order
                            {
                                OrderId = item.order_num,
                                SourceSystem = "SystemB",
                                CustomerName = item.client_name,
                                OrderDate = DateTime.Parse(item.date_placed),
                                TotalAmount = item.total,
                                Status = NormalizeStatus("SystemB", item.order_status)
                            
                            }
                        );
                    }
                }
            }
        }

        private string NormalizeStatus(string system, string? status)
        {
            switch (system)
            {
                case "SystemA":
                    return status switch
                    {
                        "PEND" => "Pending",
                        "PROC" => "Processing",
                        "SHIP" => "Shipped",
                        "COMP" => "Completed",
                        "CANC" => "Cancelled",
                        _ => "Unknown"
                    };
                case "SystemB":
                    return status switch
                    {
                        "1" => "Pending",
                        "2" => "Processing",
                        "3" => "Shipped",
                        "4" => "Completed",
                        "5" => "Cancelled",
                        _ => "Unknown"
                    };
                default:
                    return "Unknown";
            }           
        }   
    }
}
