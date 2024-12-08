using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace İdentity.Controllers
{
    public class VacancyController : Controller
    {
        [Authorize(Roles ="Hr, Director")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
