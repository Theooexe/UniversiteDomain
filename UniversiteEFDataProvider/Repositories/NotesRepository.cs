using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class NotesRepository(UniversiteDbContext context) : Repository<Notes>(context), INotesRepository
{
    public async Task AffecterNoteAsync(long idEtudiant, long idUes)
    {
        ArgumentNullException.ThrowIfNull(Context.Etudiants);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Ues);
        Etudiant e = (await Context.Etudiants.FindAsync(idEtudiant))!;
        Ue u = (await Context.Ues.FindAsync(idUes))!;
        await Context.SaveChangesAsync();
    }
    public async Task AffecterNoteAsync(Etudiant etudiant, Parcours parcours , Notes note)
    {
        await AffecterNoteAsync(etudiant.Id, parcours.Id); 
    }

    public Task<Notes?> FindNotesCompletAsync(float Valeur)
    {
        throw new NotImplementedException();
    }
}