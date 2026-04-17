namespace API_LaboBackend.DTO.Tournoi;

public class TournoiCreate
{
    public string Nom { get; set; } = null!;
    public string Lieu { get; set; } = null!;
    public int MinJoueurs { get; set; }
    public int MaxJoueurs { get; set; }
    public int? EloMin { get; set; }
    public int? EloMax { get; set; }
    public bool WomenOnly { get; set; }
    public DateTime DateFinInscriptions { get; set; }
}
