using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace osih.api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : Controller
    {
        // GET: /health
        [HttpGet(Name = "health")]
        public Health CheckHealth()
        {
            Health health = new Health
            {
                Status = "Healthy"
            };

            return health;
        }
    }
}
