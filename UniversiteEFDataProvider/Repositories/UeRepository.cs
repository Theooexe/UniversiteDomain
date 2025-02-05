using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class UeRepository(UniversiteDbContext context) : Repository<Ue>(context), IUeRepository
{
    public async Task AjouterUeAuParcoursAsync(long idUe, long idParcours)
    {
        ArgumentNullException.ThrowIfNull(Context.Ues);
        ArgumentNullException.ThrowIfNull(Context.Parcours);

        // Vérifier que l'UE et le parcours existent
        Ue ue = (await Context.Ues.FindAsync(idUe))!;
        Parcours parcours = (await Context.Parcours.FindAsync(idParcours))!;

        // Vérifier si l'UE est déjà associée au parcours
        if (ue.EnseigneeDans!.Any(p => p.Id == idParcours))
        {
            throw new InvalidOperationException("Cette UE est déjà associée à ce parcours.");
        }

        // Ajouter le parcours à la liste des parcours de l'UE
        ue.EnseigneeDans.Add(parcours);
        await Context.SaveChangesAsync();
    }

    public async Task AjouterUeAuParcoursAsync(Ue ue, Parcours parcours)
    {
        await AjouterUeAuParcoursAsync(ue.Id, parcours.Id);
    }
}