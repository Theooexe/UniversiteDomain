using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.NotesUseCases.Get;

public class GetNotesUseCase(IRepositoryFactory factory)
{
    
    public async Task<Notes?> ExecuteAsync(float Valeur)
    {
        await CheckBusinessRules();
        Notes? notes = await factory.NotesRepository().FindNotesCompletAsync(Valeur);
        //Etudiant? etudiant=await factory.EtudiantRepository().FindAsync(idEtudiant);
        return notes;
    }

    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        INotesRepository notesRepository=factory.NotesRepository();
        ArgumentNullException.ThrowIfNull(notesRepository);
    }

    public bool IsAuthorized(string role, IUniversiteUser user, long idEtudiant)
    {
        if (role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable)) return true;
        return user.Etudiant!=null && role.Equals(Roles.Etudiant) && user.Etudiant.Id==idEtudiant;
    }
}