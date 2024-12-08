using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace İdentity.Controllers
{
    public class SaleController : Controller
    {
        [Authorize(Roles ="Seller, Director")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
