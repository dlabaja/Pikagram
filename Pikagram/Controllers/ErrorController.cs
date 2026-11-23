using Microsoft.AspNetCore.Mvc;

namespace Pikagram.Controllers;

[Route("error")]
public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("{statusCode:int}")]
    public IActionResult HttpStatusCodeHandler(int statusCode)
    {
        if (statusCode == 404)
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        return View("Index", statusCode);
    }
}