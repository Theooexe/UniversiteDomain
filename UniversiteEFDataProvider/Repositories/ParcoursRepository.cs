using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository(UniversiteDbContext context) : Repository<Parcours>(context), IParcoursRepository
{
    public async Task<Parcours> AddEtudiantAsync(Parcours parcours, Etudiant etudiant)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(etudiant);

        parcours.Inscrits ??= new List<Etudiant>();

        if (!parcours.Inscrits.Contains(etudiant))
        {
            parcours.Inscrits.Add(etudiant);
            await Context.SaveChangesAsync();
        }
        return parcours;
    }

    public async Task<Parcours> AddEtudiantAsync(long idParcours, long idEtudiant)
    {
        var parcours = await Context.Parcours.Include(p => p.Inscrits).FirstOrDefaultAsync(p => p.Id == idParcours);
        var etudiant = await Context.Etudiants.FindAsync(idEtudiant);

        if (parcours == null || etudiant == null)
        {
            throw new InvalidOperationException("Parcours ou étudiant introuvable.");
        }

        return await AddEtudiantAsync(parcours, etudiant);
    }

    public async Task<Parcours> AddEtudiantAsync(Parcours? parcours, List<Etudiant> etudiants)
    {
        ArgumentNullException.ThrowIfNull(parcours);
        ArgumentNullException.ThrowIfNull(etudiants);

        parcours!.Inscrits ??= new List<Etudiant>();

        foreach (var etudiant in etudiants)
        {
            if (!parcours.Inscrits.Contains(etudiant))
            {
                parcours.Inscrits.Add(etudiant);
            }
        }

        await Context.SaveChangesAsync();
        return parcours;
    }

    public async Task<Parcours> AddEtudiantAsync(long idParcours, long[] idEtudiants)
    {
        var parcours = await Context.Parcours.Include(p => p.Inscrits).FirstOrDefaultAsync(p => p.Id == idParcours);
        var etudiants = await Context.Etudiants.Where(e => idEtudiants.Contains(e.Id)).ToListAsync();

        if (parcours == null || !etudiants.Any())
        {
            throw new InvalidOperationException("Parcours ou étudiants introuvables.");
        }

        return await AddEtudiantAsync(parcours, etudiants);
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long idUes)
    {
        var parcours = await Context.Parcours.Include(p => p.UesEnseignees).FirstOrDefaultAsync(p => p.Id == idParcours);
        var ue = await Context.Ues.FindAsync(idUes);

        if (parcours == null || ue == null)
        {
            throw new InvalidOperationException("Parcours ou UE introuvable.");
        }

        parcours.UesEnseignees ??= new List<Ue>();

        if (!parcours.UesEnseignees.Contains(ue))
        {
            parcours.UesEnseignees.Add(ue);
            await Context.SaveChangesAsync();
        }

        return parcours;
    }

    public async Task<Parcours> AddUeAsync(long idParcours, long[] idUes)
    {
        var parcours = await Context.Parcours.Include(p => p.UesEnseignees).FirstOrDefaultAsync(p => p.Id == idParcours);
        var ues = await Context.Ues.Where(u => idUes.Contains(u.Id)).ToListAsync();

        if (parcours == null || !ues.Any())
        {
            throw new InvalidOperationException("Parcours ou UEs introuvables.");
        }

        parcours.UesEnseignees ??= new List<Ue>();

        foreach (var ue in ues)
        {
            if (!parcours.UesEnseignees.Contains(ue))
            {
                parcours.UesEnseignees.Add(ue);
            }
        }

        await Context.SaveChangesAsync();
        return parcours;
    }
}