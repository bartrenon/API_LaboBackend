namespace API_LaboBackend.DTO.Tournoi;

public class TournoiShort : TournoiCreate
{
    public int Id { get; set; }
    public int RondeCourante { get; set; } = 0;
    public string Statut { get; set; } = "en attente de joueurs";
    public DateTime DateCreation { get; set; } = DateTime.UtcNow;
    public DateTime DateMiseAJour { get; set; } = DateTime.UtcNow;
}
