using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.UeExceptions;


namespace UniversiteDomain.UseCases.UeUseCases.Create;

public class CreateUeUseCase(IUeRepository ueRepository)
{
    public async Task<Ue> ExecuteAsync(string numeroUe,string intitule)
    {
        var ue = new Ue{NumeroUe = numeroUe, Intitule = intitule};
        return await ExecuteAsync(ue);
    }
    public async Task<Ue> ExecuteAsync(Ue ue)
    {
        await CheckBusinessRules(ue);
        Ue et = await ueRepository.CreateAsync(ue);
        ueRepository.SaveChangesAsync().Wait();
        return et;
    }
   
    private async Task CheckBusinessRules(Ue ue)
        {
            ArgumentNullException.ThrowIfNull(ue);
            ArgumentNullException.ThrowIfNull(ue.NumeroUe);
            ArgumentNullException.ThrowIfNull(ue.Intitule);
            ArgumentNullException.ThrowIfNull(ueRepository);
        
     
            List<Ue> existe = await ueRepository.FindByConditionAsync(e=>e.NumeroUe.Equals(ue.NumeroUe));

            if (existe .Any()) throw new DuplicateNumeroUeException(ue.NumeroUe+ " - ce numéro d'Ue est déjà affecté à une Ue");
            
        }
}