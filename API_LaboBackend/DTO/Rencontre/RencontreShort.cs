namespace API_LaboBackend.DTO.Rencontre;

public class RencontreShort : RencontreCreate
{
    public int Id { get; set; }
    public int Ronde { get; set; }
    public string? Resultat { get; set; }
}
