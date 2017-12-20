namespace BetINK.Web.Controllers
{
    using BetINK.Web.Models;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics;

    public class HomeController : Controller
    {

        public IActionResult Index()
        => View();

        public IActionResult Error()
        => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id
            ?? HttpContext.TraceIdentifier
        });

    }
}
