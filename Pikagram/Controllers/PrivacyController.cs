using Microsoft.AspNetCore.Mvc;

namespace Pikagram.Controllers;

public class PrivacyController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}