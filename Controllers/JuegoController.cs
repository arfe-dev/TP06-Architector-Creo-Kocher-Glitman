using Microsoft.AspNetCore.Mvc;
using TP06.Models;

namespace TP06.Controllers;

public class JuegoController : Controller
{
    public IActionResult Index()
    {
        ViewBag.Mensaje = TempData["Mensaje"];
        return View();
    }

    [HttpPost]
public IActionResult Index(string nombre, string equipo)
{
    if (string.IsNullOrWhiteSpace(nombre))
    {
        ViewBag.Mensaje = "Ingresá tu nombre para comenzar.";
        return RedirectToAction("Index");
    }

    if (string.IsNullOrWhiteSpace(equipo))
    {
        ViewBag.Mensaje = "Elegí un equipo para comenzar.";
        return RedirectToAction("Index");
    }

    BD bd = new BD();

    int idPartida = bd.CrearPartida(nombre, equipo);

    HttpContext.Session.SetInt32("PartidaId", idPartida);
    HttpContext.Session.SetString("Nombre", nombre);
    HttpContext.Session.SetString("Equipo", equipo);

    return RedirectToAction("Octavos");
}


    public IActionResult Octavos()
    {
        string equipo = HttpContext.Session.GetString("Equipo");

        if (string.IsNullOrEmpty(equipo))
        {
            ViewBag.Mensaje = "Primero tenés que elegir un equipo.";
            return RedirectToAction("Index");
        }

        ViewBag.Equipo = equipo;
        ViewBag.Imagen = equipo + ".png";

        BD bd = new BD();

        List<Pregunta> preguntas = bd.ObtenerPreguntas();

        HttpContext.Session.SetString("OctavosRespuestaCorrecta1",preguntas[0].RespuestaCorrecta);

        HttpContext.Session.SetString("OctavosRespuestaCorrecta2", preguntas[1].RespuestaCorrecta);

        HttpContext.Session.SetString("OctavosRespuestaCorrecta3", preguntas[2].RespuestaCorrecta);

        return View(preguntas);
    }


    [HttpPost]
    public IActionResult Octavos(
        string respuesta1, string respuesta2, string respuesta3)
    {
        string respuestaCorrecta1 = HttpContext.Session.GetString("OctavosRespuestaCorrecta1");

        string respuestaCorrecta2 = HttpContext.Session.GetString("OctavosRespuestaCorrecta2");

        string respuestaCorrecta3 = HttpContext.Session.GetString("OctavosRespuestaCorrecta3");

        if (respuesta1 == respuestaCorrecta1 && respuesta2 == respuestaCorrecta2 && respuesta3 == respuestaCorrecta3)
        {
            HttpContext.Session.SetString("OctavosSuperados", "true");

            HttpContext.Session.SetString("Fase", "2");

            return RedirectToAction("Cuartos");
        }

        ViewBag.Mensaje = "Quedaste eliminado. Elegí otro equipo para volver a empezar.";

        return RedirectToAction("Index", "Home");
    }


    public IActionResult Cuartos()
    {
        string octavosSuperados = HttpContext.Session.GetString("OctavosSuperados");

        if (octavosSuperados != "true")
        {
            ViewBag.Mensaje = "No podés entrar a Cuartos sin superar Octavos.";

            return RedirectToAction("Index", "Home");
        }

        string palabra = HttpContext.Session.GetString("CuartosPalabra");

        if (string.IsNullOrEmpty(palabra))
        {
            BD bd = new BD();

            Ahorcado ahorcado = bd.ObtenerPalabraAhorcado();

            palabra = ahorcado.Palabra.ToUpper();

            HttpContext.Session.SetString("CuartosPalabra", palabra);
            HttpContext.Session.SetString("CuartosLetras", "");
            HttpContext.Session.SetInt32("CuartosIntentos", 6);
        }

        ViewBag.PalabraOculta = ObtenerPalabraOculta(palabra, HttpContext.Session.GetString("CuartosLetras"));

        ViewBag.Intentos = HttpContext.Session.GetInt32("CuartosIntentos").Value;

        return View();
    }


    [HttpPost]
    public IActionResult Cuartos(string letra)
    {
        string octavosSuperados = HttpContext.Session.GetString("OctavosSuperados");

        if (octavosSuperados != "true")
        {
            return RedirectToAction("Index", "Home");
        }

        string palabra = HttpContext.Session.GetString("CuartosPalabra");

        if (string.IsNullOrEmpty(palabra))
        {
            return RedirectToAction("Cuartos");
        }

        if (string.IsNullOrWhiteSpace(letra))
        {
            ViewBag.Mensaje = "Ingresá una letra.";
        }
        else
        {
            letra = letra.ToUpper();

            string letras = HttpContext.Session.GetString("CuartosLetras");

            if (!letras.Contains(letra))
            {
                letras += letra;

                HttpContext.Session.SetString("CuartosLetras", letras);

                if (!palabra.Contains(letra))
                {
                    int intentos = HttpContext.Session.GetInt32("CuartosIntentos").Value;

                    intentos--;

                    HttpContext.Session.SetInt32( "CuartosIntentos", intentos);
                }
            }
        }

        string letrasActuales = HttpContext.Session.GetString("CuartosLetras");

        int intentosActuales = HttpContext.Session.GetInt32("CuartosIntentos").Value;

        string palabraOculta = ObtenerPalabraOculta(palabra, letrasActuales);

        ViewBag.PalabraOculta = palabraOculta;
        ViewBag.Intentos = intentosActuales;

        if (!palabraOculta.Contains("_"))
        {
            HttpContext.Session.SetString("CuartosSuperados", "true");

            HttpContext.Session.SetString("Fase", "3");

            return RedirectToAction("Semis");
        }

        if (intentosActuales <= 0)
        {
            ViewBag.Mensaje = "Perdiste el ahorcado. Volvé a empezar.";

            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    private string ObtenerPalabraOculta(string palabra, string letras)
    {
        string resultado = "";

        foreach (char caracter in palabra)
        {
            if (caracter == ' ')
            {
                resultado += "  ";
            }
            else if (letras.Contains(caracter))
            {
                resultado += caracter + " ";
            }
            else
            {
                resultado += "_ ";
            }
        }

        return resultado;
    }


    public IActionResult Semis()
    {
        string cuartosSuperados = HttpContext.Session.GetString("CuartosSuperados");

        if (cuartosSuperados != "true")
        {
            ViewBag.Mensaje = "No podés entrar a Semifinal sin superar Cuartos.";

            return RedirectToAction("Index", "Home");
        }

        BD bd = new BD();

        Jugador jugador = bd.ObtenerJugador();

        if (jugador == null)
        {
            ViewBag.Mensaje = "No hay jugadores disponibles.";

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
        string cuartosSuperados = HttpContext.Session.GetString("CuartosSuperados");

        if (cuartosSuperados != "true")
        {
            return RedirectToAction("Index", "Home");
        }

        string nombre = HttpContext.Session.GetString("JugadorSemis");

        if (!string.IsNullOrWhiteSpace(respuesta) && respuesta.ToLower() == nombre.ToLower())
        {
            HttpContext.Session.SetString("SemisSuperadas", "true");

            HttpContext.Session.SetString("Fase", "4");

            return RedirectToAction("Final");
        }

        ViewBag.Mensaje = "Respuesta incorrecta.";

        return Semis();
    }


    public IActionResult Final()
    {
        string semisSuperadas = HttpContext.Session.GetString("SemisSuperadas");

        if (semisSuperadas != "true")
        {
            ViewBag.Mensaje = "No podés entrar a la Final sin superar Semifinal.";

            return RedirectToAction("Index", "Home");
        }

        BD bd = new BD();

        CodigoFinal codigo = bd.ObtenerCodigoFinal();

        HttpContext.Session.SetString("CodigoCorrecto", codigo.CodigoCorrecto.ToString());
        HttpContext.Session.SetString("Pista1", codigo.Pista1);
        HttpContext.Session.SetString("Pista2", codigo.Pista2);
        HttpContext.Session.SetString("Pista3", codigo.Pista3);

        ViewBag.Pista1 = codigo.Pista1;
        ViewBag.Pista2 = codigo.Pista2;
        ViewBag.Pista3 = codigo.Pista3;

        return View();
    }


    [HttpPost]
    public IActionResult Final(int respuesta)
    {
        string semisSuperadas = HttpContext.Session.GetString("SemisSuperadas");

        if (semisSuperadas != "true")
        {
            return RedirectToAction("Index", "Home");
        }

        string codigoCorrecto = HttpContext.Session.GetString("CodigoCorrecto");

        ViewBag.Pista1 = HttpContext.Session.GetString("Pista1");
        ViewBag.Pista2 = HttpContext.Session.GetString("Pista2");
        ViewBag.Pista3 = HttpContext.Session.GetString("Pista3");

        if (respuesta.ToString() == codigoCorrecto)
        {
            HttpContext.Session.SetString("FinalSuperada", "true");

            HttpContext.Session.SetString("Fase", "5");

            return RedirectToAction("ResultadoCorrecto");
        }

        ViewBag.Mensaje = "Código incorrecto.";

        return View();
    }


    public IActionResult ResultadoCorrecto()
    {
        string finalSuperada = HttpContext.Session.GetString("FinalSuperada");

        if (finalSuperada != "true")
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }


    public IActionResult FinDeJuego()
    {
        string finalSuperada = HttpContext.Session.GetString("FinalSuperada");

        if (finalSuperada != "true")
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }
}