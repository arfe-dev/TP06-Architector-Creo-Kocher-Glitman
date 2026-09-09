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
}
