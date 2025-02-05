using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NotesExceptions;

namespace UniversiteDomain.UseCases.NotesUseCases.Create
{
    public class CreateNotesUseCase
    {
        private readonly IRepositoryFactory factory;

        public CreateNotesUseCase(IRepositoryFactory factory)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        // Méthode qui prend une valeur pour la note et crée une note
        public async Task<Notes> ExecuteAsync(float valeur, long etudiantId, long ueId)
        {
            var notes = new Notes
            {
                Valeur = valeur,
                EtudiantId = etudiantId,
                UeId = ueId
            };

            return await ExecuteAsync(notes);
        }

        // Méthode qui prend directement une entité Notes
        public async Task<Notes> ExecuteAsync(Notes notes)
        {
            await CheckBusinessRules(notes);

            
            var existingNotes = await factory.NotesRepository()
                                    .FindByConditionAsync(n => n.EtudiantId == notes.EtudiantId && n.UeId == notes.UeId) 
                                ?? new List<Notes>(); 

    
            if (existingNotes.Any()) 
            {
                throw new NoteDejaExistanteException("L'étudiant a déjà une note pour cette UE.");
            }

            // Créer la nouvelle note
            Notes createdNote = await factory.NotesRepository().CreateAsync(notes);
            await factory.NotesRepository().SaveChangesAsync();
            return createdNote;
        }

        // Vérification des règles métiers
        private async Task CheckBusinessRules(Notes notes)
        {
            ArgumentNullException.ThrowIfNull(notes);
            ArgumentNullException.ThrowIfNull(notes.Valeur);

            // Vérification de la valeur de la note
            if (notes.Valeur < 0 || notes.Valeur > 20)
            {
                throw new ValeurIncorrecteException("La note doit être comprise entre 0 et 20.");
            }

            // Vérification de la présence du repository
            ArgumentNullException.ThrowIfNull(factory.NotesRepository());
        }
    }
}
