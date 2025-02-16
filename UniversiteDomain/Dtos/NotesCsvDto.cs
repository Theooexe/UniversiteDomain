namespace UniversiteDomain.Dtos;

public class NotesCsvDto
{
    public long IdEtudiant { get; set; }
    public string NumeroEtudiant { get; set; }
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public string Email { get; set; }
    public float Note { get; set; }  // Note peut être null (cellule vide)
    public string IntituleUE { get; set; }
}