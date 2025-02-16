using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;


namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IRepositoryFactory factory)
{
    public async Task<Ue> ExecuteAsync(string numeroUe,string intitule)
    {
        var ue = new Ue{NumeroUe = numeroUe, Intitule = intitule};
        return await ExecuteAsync(ue);
    }
    public async Task<Ue> ExecuteAsync(Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue et = await factory.UeRepository().CreateAsync(ue);
        factory.UeRepository().SaveChangesAsync().Wait();
        return et;
    }
   
    private async Task CheckBusinessRules(Ue ue)
        {
            ArgumentNullException.ThrowIfNull(ue);
            ArgumentNullException.ThrowIfNull(ue.NumeroUe);
            ArgumentNullException.ThrowIfNull(ue.Intitule);
            
            List<Ue> existe = await factory.UeRepository().FindByConditionAsync(e=>e.NumeroUe.Equals(ue.NumeroUe));

            if (existe .Any()) throw new DuplicateNumeroUeException(ue.NumeroUe+ " - ce numéro d'Ue est déjà affecté à une Ue");
            
        }

    public bool IsAuthorized(string role)
    {
        throw new NotImplementedException();
    }
}