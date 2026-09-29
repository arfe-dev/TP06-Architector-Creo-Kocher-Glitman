namespace TP06.Models;

public class Partida
{
    public int Id { get; set; }
    public string Descripcion { get; set; }
    public string NombreDelParticipante { get; set; }
    public string Equipo { get; set; }
    public int Fase { get; set; }
}