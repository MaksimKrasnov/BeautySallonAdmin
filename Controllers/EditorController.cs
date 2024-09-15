using Microsoft.AspNetCore.Mvc;

namespace BeautySallonAdmin.Controllers
{
    public class EditorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
