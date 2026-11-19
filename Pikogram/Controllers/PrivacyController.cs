using Microsoft.AspNetCore.Mvc;

namespace Pikogram.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}