using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class JuegoController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string equipo)
    {
        HttpContext.Session.SetString("Equipo", equipo);
        return RedirectToAction("Octavos");
    }

    public IActionResult Octavos()
    {
        return View();
    }

    public IActionResult Cuartos()
    {
        return View();
    }

    public IActionResult Semis()
    {
        return View();
    }

    public IActionResult Final()
    {
        return View();
    }

    public IActionResult ResultadoCorrecto()
    {
        return View();
    }

    public IActionResult FinDeJuego()
    {
        return View();
    }
}