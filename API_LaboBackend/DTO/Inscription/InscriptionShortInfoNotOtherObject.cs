namespace API_LaboBackend.DTO.Inscription;

public class InscriptionShortInfoNotOtherObject
{
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;
    public int JoueurId { get; set; }
    public int TournoiId { get; set; }
}
