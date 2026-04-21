namespace BLL.Dto;

public class Score
{
    public string Nom { get; set; } = default!;
    public int MatchsJoues { get; set; }
    public int Victoires { get; set; }
    public int Defaites { get; set; }
    public int Egalites { get; set; }
    public double ScoreFinal { get; set; }
}
