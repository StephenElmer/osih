using osih.model;
using System.ComponentModel.DataAnnotations;

namespace osih.web.Models
{
    public class HomeViewModel
    {
        public List<Order> Orders { get; set; } = new();

        public Order? SelectedOrder { get; set; } = null;

        public List<string> Statuses { get; set; } = new() { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

        [Display(Name = "Status")]
        [DataType(DataType.Text)]
        [StringLength(100)]
        public string SelectedStatus { get; set; } = string.Empty;

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public string SelectedStartDate { get; set; } = string.Empty;

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public string SelectedEndDate { get; set; } = string.Empty;
    }
}
