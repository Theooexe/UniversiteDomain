using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NotesExceptions;

namespace UniversiteDomain.UseCases.NotesUseCases.Create;

public class CreateNotesUseCase(IRepositoryFactory factory)
{
     public async Task<Notes> ExecuteAsync(float valeur)
    {
        var notes = new Notes{Valeur = valeur};
        return await ExecuteAsync(notes);
    }
    public async Task<Notes> ExecuteAsync(Notes notes)
    {
        await CheckBusinessRules(notes);
        Notes et = await factory.NotesRepository().CreateAsync(notes);
        factory.SaveChangesAsync().Wait();
        return et;
    }
   
    private async Task CheckBusinessRules(Notes notes)
        {
            ArgumentNullException.ThrowIfNull(notes);
            ArgumentNullException.ThrowIfNull(notes.Valeur);
            ArgumentNullException.ThrowIfNull(factory.NotesRepository());

            if (notes.Valeur < 0 || notes.Valeur > 20)
            {
                throw new ValeurIncorrecteException("La note doit être comprise entre 0 et 20.");
            }
        }
}