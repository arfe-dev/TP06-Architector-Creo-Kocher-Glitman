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
            string query = @"SELECT TOP 3 * FROM Preguntas ORDER BY NEWID()";

            return connection.Query<Pregunta>(query).ToList();
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
            string query = @"SELECT TOP 1 * FROM CodigoFinal";

            return connection.QueryFirstOrDefault<CodigoFinal>(query);
        }
    }
}
