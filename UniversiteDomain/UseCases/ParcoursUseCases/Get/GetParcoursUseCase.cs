using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.ParcoursUseCases.Get;

public class GetParcoursUseCase(IRepositoryFactory factory)
{
    public async Task<Parcours?> ExecuteAsync(long Id)
    {
        await CheckBusinessRules();
        Parcours? parcours = await factory.ParcoursRepository().FindParcoursAsync(Id);
        return parcours;
    }
    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IParcoursRepository parcoursRepository=factory.ParcoursRepository();
        ArgumentNullException.ThrowIfNull(parcoursRepository);
    }
    public bool IsAuthorized(string role, IUniversiteUser user, long idEtudiant)
    {
        if (role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable)) return true;
        return user.Etudiant!=null && role.Equals(Roles.Etudiant) && user.Etudiant.Id==idEtudiant;
    }
}