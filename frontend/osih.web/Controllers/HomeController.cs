using Microsoft.AspNetCore.Mvc;
using osih.web.Models;
using System.Diagnostics;
using osih.model;
using System.Threading.Tasks;

namespace osih.web.Controllers
{
    public class HomeController : Controller
    {
        private IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? orderId)
        {
            HomeViewModel model = new HomeViewModel();

            var client = _httpClientFactory.CreateClient("OrdersApi");
            var response = await client.GetAsync("orders").ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<Order>>().ConfigureAwait(false);
                model.Orders = orders ?? new List<Order>();

                if (!String.IsNullOrWhiteSpace(orderId))
                {
                    Order? o = orders?.Where(x => x.OrderId == orderId).FirstOrDefault();
                    if (o != null)
                    {
                        model.SelectedOrder = o;
                    }
                }
            }
            else
            {
                model.Orders = new List<Order>();
            }


            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeViewModel data)
        {

            string? status = data.SelectedStatus;
            string? startDate = data.SelectedStartDate;
            string? endDate = data.SelectedEndDate;

            var client = _httpClientFactory.CreateClient("OrdersApi");

            HomeViewModel model = new HomeViewModel();

            HttpResponseMessage? response;
            if (!String.IsNullOrEmpty(status))
            {
                response = await client.GetAsync("orders/search?status=" + status).ConfigureAwait(false);
            }
            else if (!String.IsNullOrWhiteSpace(startDate) && !String.IsNullOrWhiteSpace(endDate))
            {
                response = await client.GetAsync("orders/search?startDate=" + startDate + "&endDate=" + endDate).ConfigureAwait(false);
            }
            else
            {
                response = await client.GetAsync("orders").ConfigureAwait(false);
            }

            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<Order>>().ConfigureAwait(false);
                model.Orders = orders ?? new List<Order>();
            }
            else
            {
                model.Orders = new List<Order>();
            }





            return View(model);

        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
