using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Organizer.Controllers
{
    [Authorize]
    public class PlannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
