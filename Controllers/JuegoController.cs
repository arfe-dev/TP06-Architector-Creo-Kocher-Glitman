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

        HttpContext.Session.SetString("OctavosRespuestaCorrecta1", preguntas[0].RespuestaCorrecta);
        HttpContext.Session.SetString("OctavosRespuestaCorrecta2", preguntas[1].RespuestaCorrecta);
        HttpContext.Session.SetString("OctavosRespuestaCorrecta3", preguntas[2].RespuestaCorrecta);

        return View(preguntas);
    }

    [HttpPost]
    public IActionResult Octavos(string respuesta1, string respuesta2, string respuesta3)
    {
        string respuestaCorrecta1 = HttpContext.Session.GetString("OctavosRespuestaCorrecta1");
        string respuestaCorrecta2 = HttpContext.Session.GetString("OctavosRespuestaCorrecta2");
        string respuestaCorrecta3 = HttpContext.Session.GetString("OctavosRespuestaCorrecta3");

        if (respuesta1 == respuestaCorrecta1 && respuesta2 == respuestaCorrecta2 && respuesta3 == respuestaCorrecta3)
        {
            return RedirectToAction("Cuartos");
        }

        TempData["Mensaje"] = "Quedaste eliminado. Elegí otro equipo para volver a empezar.";
        return RedirectToAction("Index", "Home");
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

        if (jugador == null)
        {
            TempData["Mensaje"] = "Error: No hay jugadores disponibles en la base de datos.";
            return RedirectToAction("Index", "Home");
        }

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

        if (respuesta.ToLower() == nombre.ToLower())    
        {   
            return RedirectToAction("Final");
        }

        ViewBag.Mensaje = "Respuesta incorrecta";
        return View();
    }   


   public IActionResult Final()
{
    CodigoFinal codigo = bd.ObtenerCodigoFinal();

    ViewBag.Pista1 = codigo.Pista1;
    ViewBag.Pista2 = codigo.Pista2;
    ViewBag.Pista3 = codigo.Pista3;

    return View();
}
    [HttpPost]
public IActionResult Final(int respuesta)
{
    int codigoCorrecto = HttpContext.Session.GetInt32("CodigoCorrecto").Value;

    if (respuesta == codigoCorrecto)
    {
        return RedirectToAction("ResultadoCorrecto");
    }

    ViewBag.Mensaje = "Código incorrecto";

    return Final();
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