using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HospitalManagement.Controllers;

[Authorize]
public class MenuController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
