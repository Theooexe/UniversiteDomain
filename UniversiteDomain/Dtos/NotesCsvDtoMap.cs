using CsvHelper.Configuration;
using UniversiteDomain.Dtos;

public class NotesCsvDtoMap : ClassMap<NotesCsvDto>
{
    public NotesCsvDtoMap()
    {
        Map(m => m.IdEtudiant).Index(0);
        Map(m => m.NumeroEtudiant).Index(1);
        Map(m => m.Nom).Index(2);
        Map(m => m.Prenom).Index(3);
        Map(m => m.Email).Index(4);
        Map(m => m.Note).Index(5)
            .TypeConverterOption.NullValues("")  
            .TypeConverter<FloatConverter>();  
        Map(m => m.IntituleUE).Index(6);
    }
}