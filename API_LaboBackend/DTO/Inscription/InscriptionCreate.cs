namespace API_LaboBackend.DTO.Inscription;

public class InscriptionCreate
{
    public int JoueurId { get; set; }
    public int TournoiId { get; set; }
    public DateTime DateInscription { get; set; } = DateTime.UtcNow;

}
