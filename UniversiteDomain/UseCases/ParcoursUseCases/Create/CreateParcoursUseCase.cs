using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Util;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Create;

public class CreateParcoursUseCase(IParcoursRepository parcoursRepository)
{
    public async Task<Parcours> ExecuteAsync(string nomParcours,int anneeFormation)
    {
        var parcours = new Parcours{NomParcours = nomParcours, AnneeFormation = anneeFormation};
        return await ExecuteAsync(parcours);
    }
    public async Task<Parcours> ExecuteAsync(Parcours parcours)
    {
        await CheckBusinessRules(parcours);
        Parcours et = await parcoursRepository.CreateAsync(parcours);
        parcoursRepository.SaveChangesAsync().Wait();
        return et;
    }
   
    private async Task CheckBusinessRules(Parcours parcours)
        {
            ArgumentNullException.ThrowIfNull(parcours);
            ArgumentNullException.ThrowIfNull(parcours.NomParcours);
            ArgumentNullException.ThrowIfNull(parcours.AnneeFormation);
            ArgumentNullException.ThrowIfNull(parcoursRepository);
        
            // On recherche un parcours avec le meme nom de parcours
            List<Parcours> existe = await parcoursRepository.FindByConditionAsync(e=>e.NomParcours.Equals(parcours.NomParcours));

            // Si un parcours avec le même numéro de parcours existe déjà, on lève une exception personnalisée
            if (existe .Any()) throw new DuplicateNumEtudException(parcours.NomParcours+ " - ce numéro de parcours est déjà affecté à un parcours");
            
            // Le métier définit que la date doit etre comrpise entre 2000 et 2100
            if (parcours.AnneeFormation < 2000 || parcours.AnneeFormation > 2100)
                throw new InvalidNomEtudiantException(parcours.AnneeFormation + " incorrect - L'année de formation doit être un entier de 4 chiffres");

        }
}