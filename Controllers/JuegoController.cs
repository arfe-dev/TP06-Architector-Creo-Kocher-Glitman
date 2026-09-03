using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrimerProyecto.Models;

namespace PrimerProyecto.Controllers;

public class JuegoController : Controller
{
    public IActionResult Octavos()
{
    return View();
}

public IActionResult Cuartos()
{
    return View();
}

public IActionResult Semifinal()
{
    return View();
}

public IActionResult Final()
{
    return View();
}

public IActionResult Ganador()
{
    return View();
}
}