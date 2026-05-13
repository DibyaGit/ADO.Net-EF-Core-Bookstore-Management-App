using Microsoft.AspNetCore.Mvc;

namespace BookstoreApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}