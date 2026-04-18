using API_LaboBackend.DTO.Inscription;
using Domain.Entities;

namespace API_LaboBackend.Mappers;

public class InscriptionMapper
{
    public static Inscription ToInscription(InscriptionCreate i)
    {
        return new Inscription
        {
            JoueurId = i.JoueurId,
            TournoiId = i.TournoiId,
            DateInscription = i.DateInscription
        };
    }
}
