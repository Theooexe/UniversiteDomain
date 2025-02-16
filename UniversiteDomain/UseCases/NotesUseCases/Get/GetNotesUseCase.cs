using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.NotesUseCases.Get;

public class GetNotesUseCase(IRepositoryFactory repositoryFactory)
{
    public bool IsAuthorized(string role, IUniversiteUser user, long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Parcours?> ExecuteAsync(long id)
    {
        throw new NotImplementedException();
    }
}