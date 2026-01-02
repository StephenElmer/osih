namespace osih.model
{
    public class Order
    {
        public string? OrderId { get; set; }

        public string? SourceSystem { get; set; }

        public string? CustomerName { get; set; }

        public DateTime? OrderDate { get; set; }

        public double? TotalAmount { get; set; }

        public string? Status { get; set; }
    }
}
