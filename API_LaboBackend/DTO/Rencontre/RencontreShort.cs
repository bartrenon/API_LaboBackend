using System.Text.Json.Serialization;

namespace API_LaboBackend.DTO.Rencontre;

public class RencontreShort 
{
    public int Id { get; set; }
     public int TournoiId { get; set; }

    public int JoueurBlancId { get; set; }

    public int JoueurNoirId { get; set; }
    public int Ronde { get; set; }
    public string? Resultat { get; set; }
}
