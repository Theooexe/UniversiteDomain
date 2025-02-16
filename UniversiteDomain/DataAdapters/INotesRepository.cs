using UniversiteDomain.Entities;

namespace UniversiteDomain.DataAdapters;

public interface INotesRepository: IRepository<Notes>
{
    Task<Notes?> FindNotesCompletAsync(float Valeur);
    Task AddNoteAsync(long ueId, long etudiantId, object notesE);
}