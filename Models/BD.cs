using Dapper;
using Microsoft.Data.SqlClient;
using TP06.Models;

namespace TP06.Models;


public class BD{
    private string _connectionString = @"Server=localhost; DataBase=TP06; Integrated Security=True;TrustServerCertificate=True;";

   public List<Pregunta> ObtenerPreguntas()
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM Preguntas";

        List<Pregunta> preguntas = connection.Query<Pregunta>(query).ToList();

        Random random = new Random();

        List<Pregunta> preguntasElegidas = new List<Pregunta>();

        while (preguntasElegidas.Count < 3)
        {
            int numero = random.Next(0, preguntas.Count);

            if (!preguntasElegidas.Contains(preguntas[numero]))
            {
                preguntasElegidas.Add(preguntas[numero]);
            }
        }

        foreach (Pregunta pregunta in preguntasElegidas)
        {
            pregunta.Opciones = new List<string>();

            pregunta.Opciones.Add(pregunta.OpcionA);
            pregunta.Opciones.Add(pregunta.OpcionB);
            pregunta.Opciones.Add(pregunta.OpcionC);
            pregunta.Opciones.Add(pregunta.OpcionD);
        }

        return preguntasElegidas;
    }



    }  
    public Equipo ObtenerEquipo(string nombre)
    {
        Equipo equipo = null;

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT * FROM Equipo WHERE Nombre = @nombre";

            equipo = connection.QueryFirstOrDefault<Equipo>(
                query,
                new { nombre = nombre }
            );
        }

        return equipo;
    }

    public List<Equipo> ObtenerEquipos()
    {
        List<Equipo> equipos = new List<Equipo>();

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT * FROM Equipo";

            equipos = connection.Query<Equipo>(query).ToList();
        }

        return equipos;
    }


    public Ahorcado ObtenerPalabraAhorcado()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT TOP 1 * FROM Ahorcado ORDER BY NEWID()";

            return connection.QueryFirstOrDefault<Ahorcado>(query);
        }
    }


    public Jugador ObtenerJugador()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT TOP 1 * FROM Jugadores ORDER BY NEWID()";

            return connection.QueryFirstOrDefault<Jugador>(query);
        }
    }

public CodigoFinal ObtenerCodigoFinal()
{
    using (SqlConnection connection = new SqlConnection(_connectionString))
    {
        string query = "SELECT * FROM CodigoFinal";

        List<CodigoFinal> codigos = connection.Query<CodigoFinal>(query).ToList();

        Random random = new Random();

        int numero = random.Next(0, codigos.Count);

        return codigos[numero];
    }
}

}
