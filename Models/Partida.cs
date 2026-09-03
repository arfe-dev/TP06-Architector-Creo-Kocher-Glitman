namespace PrimerProyecto.Models;

public class Partida
{
    public Equipo EquipoElegido { get; set; }

    public int Fase { get; set; }

    public bool OctavosSuperados { get; set; }
    public bool CuartosSuperados { get; set; }
    public bool SemifinalSuperada { get; set; }
    public bool FinalSuperada { get; set; }
}