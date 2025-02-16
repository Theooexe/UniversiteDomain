using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Get;

public class GetUeUseCase(IRepositoryFactory repositoryFactory)
{
    public bool IsAuthorized(string role, IUniversiteUser user, long id)
    {
        throw new NotImplementedException();
    }

    public async Task<Ue?> ExecuteAsync(long id)
    {
        throw new NotImplementedException();
    }
}