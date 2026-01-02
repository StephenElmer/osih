using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using osih.data;
using osih.model;

namespace osih.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        // GET: /api/orders
        [HttpGet]
        public ActionResult<IEnumerable<Order>> Orders()
        {
            DataAccess db = new DataAccess();
            var result = db.Orders;
            return result;
        }

        // GET: /api/orders/{orderId}
        [HttpGet("{orderId}")]
        public ActionResult<Order> OrderDetails(string orderId)
        {
            DataAccess db = new DataAccess();
            var result = db.Orders.Where(o => o.OrderId == orderId).FirstOrDefault();
            return result;
        }

        // GET: /api/orders/search?status={status}
        [HttpGet("search")]
        public ActionResult<IEnumerable<Order>> OrdersStatusSearch([FromQuery] string? status, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            DataAccess db = new DataAccess();
            
            var query = db.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= startDate.Value && o.OrderDate <= endDate.Value);
            }

            return Ok(query.ToList());
        }
    }
}
