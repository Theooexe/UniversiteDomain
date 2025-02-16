using UniversiteDomain.Entities;

namespace UniversiteDomain.Dtos;

public class NotesDto
{
    public float Valeur { get; set; }

    public NotesDto ToDto(Notes notes)
    {
        Valeur = notes.Valeur;
        return this;
    }

    public Notes ToEntity()
    {
        return new Notes{Valeur = Valeur};
    }
}