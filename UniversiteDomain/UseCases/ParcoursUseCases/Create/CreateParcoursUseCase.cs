using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.ParcoursExceptions;


namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IRepositoryFactory factory)
{
    public async Task<Parcours> ExecuteAsync(string nomParcours,int anneeFormation)
    {
        var parcours = new Parcours{NomParcours = nomParcours, AnneeFormation = anneeFormation};
        return await ExecuteAsync(parcours);
    }
    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours et = await factory.ParcoursRepository().CreateAsync(parcours);
        factory.SaveChangesAsync().Wait();
        return et;
    }
   
    private async Task CheckBusinessRules(Parcours parcours)
        {
            ArgumentNullException.ThrowIfNull(parcours);
            ArgumentNullException.ThrowIfNull(parcours.NomParcours);
            ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
            ArgumentNullException.ThrowIfNull(factory.ParcoursRepository());
        
            // On recherche un parcours avec le meme nom de parcours
            List<Parcours> existe = await factory.ParcoursRepository().FindByConditionAsync(e=>e.NomParcours.Equals(parcours.NomParcours));

            // Si un parcours avec le même nom de parcours existe déjà, on lève une exception personnalisée
            if (existe .Any()) throw new DuplicateNomParcoursException(parcours.NomParcours+ " - ce nom de parcours existe déja");
            
            // L'année de dormation doit etre en 1 et 2
            if (parcours.AnneeFormation < 1 || parcours.AnneeFormation > 2)
                throw new InvalidAnneeFormationException(parcours.AnneeFormation + " incorrect - L'année de formation doit être entre 1 et 2");

        }
}