using Microsoft.AspNetCore.Mvc;

namespace Pikogram.Controllers;

public class SearchController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}