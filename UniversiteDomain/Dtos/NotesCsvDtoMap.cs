using CsvHelper.Configuration;
namespace UniversiteDomain.Dtos;

public sealed class NotesCsvDtoMap : ClassMap<NotesCsvDto>
{
    public NotesCsvDtoMap()
    {
        Map(m => m.IdEtudiant).Name("IdEtudiant");
        Map(m => m.NumeroEtudiant).Name("NumeroEtudiant");
        Map(m => m.Nom).Name("Nom");
        Map(m => m.Prenom).Name("Prenom");
        Map(m => m.Email).Name("Email");
        Map(m => m.Note).Name("Note").Optional();
        Map(m => m.IntituleUE).Name("IntituleUE");
    }
}