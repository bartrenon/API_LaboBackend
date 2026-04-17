namespace API_LaboBackend.DTO.Tournoi;

public class TournoiShortInfo
{
    public string Nom { get; set; } = null!;
    public string Lieu { get; set; } = null!;
    public DateTime DateFinInscriptions { get; set; }
    public string Statut { get; set; } = "en attente de joueurs";
    public DateTime DateMiseAJour { get; set; }
}
