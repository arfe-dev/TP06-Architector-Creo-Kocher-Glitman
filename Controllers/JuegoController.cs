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
    string equipo = HttpContext.Session.GetString("Equipo");

    ViewBag.Equipo = equipo;
    ViewBag.Imagen = equipo + ".png";

    BD bd = new BD();

    List<Pregunta> preguntas = bd.ObtenerPreguntas();

    return View(preguntas);
}

    public IActionResult Cuartos()
{

    BD bd = new BD();

    Ahorcado ahorcado = bd.ObtenerPalabraAhorcado();

    ViewBag.Palabra = ahorcado.Palabra;

    return View();

}

    public IActionResult Semis()
{
    BD bd = new BD();

    Jugador jugador = bd.ObtenerJugador();

    HttpContext.Session.SetString("JugadorSemis", jugador.Nombre);

    ViewBag.Nacionalidad = jugador.Nacionalidad;
    ViewBag.Posicion = jugador.Posicion;
    ViewBag.Club = jugador.Club;

    return View();

    }

    [HttpPost]
    public IActionResult Semis(string respuesta)
    {
        string nombre = HttpContext.Session.GetString("JugadorSemis");

        if (respuesta == nombre)
        {   
            return RedirectToAction("Final");
        }

        ViewBag.Mensaje = "Respuesta incorrecta";
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