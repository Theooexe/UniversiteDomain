namespace UniversiteDomain.Entities;

public class Etudiant
{
    public long Id { get; set; }
    public string NumEtud { get; set; } = string.Empty;
    public string Nom { get; set; } = string.Empty;
    public string Prenom { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public ICollection<Notes> NotesObtenues { get; set; } = new List<Notes>();
    public Parcours? ParcoursSuivi { get; set; } = null;
    public override string ToString()
    {
        return $"ID {Id} : {NumEtud} - {Nom} {Prenom} inscrit en "+ParcoursSuivi;
    }
}