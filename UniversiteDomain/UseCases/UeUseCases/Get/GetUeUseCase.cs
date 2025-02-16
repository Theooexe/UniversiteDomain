using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;

namespace UniversiteDomain.UseCases.UeUseCases.Get;

public class GetUeUseCase(IRepositoryFactory factory)
{
    public async Task<Ue?> ExecuteAsync(long Id)
    {
        await CheckBusinessRules();
        Ue? ue = await factory.UeRepository().FindUeAsync(Id);
        return ue;
    }
    
    private async Task CheckBusinessRules()
    {
        ArgumentNullException.ThrowIfNull(factory);
        IUeRepository parcoursRepository=factory.UeRepository();
        ArgumentNullException.ThrowIfNull(parcoursRepository);
    }
    public bool IsAuthorized(string role, IUniversiteUser user, long idEtudiant)
    {
        if (role.Equals(Roles.Scolarite) || role.Equals(Roles.Responsable)) return true;
        return user.Etudiant!=null && role.Equals(Roles.Etudiant) && user.Etudiant.Id==idEtudiant;
    }
}