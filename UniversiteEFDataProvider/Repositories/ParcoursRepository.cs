using Microsoft.EntityFrameworkCore;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.Entities;
using UniversiteEFDataProvider.Data;

namespace UniversiteEFDataProvider.Repositories;

public class ParcoursRepository : Repository<Parcours>, IParcoursRepository
    {
        public ParcoursRepository(UniversiteDbContext context) : base(context)
        {
        }

        // Méthode pour ajouter un nouvel étudiant dans un parcours
        public async Task AjouterEtudiantDansParcoursAsync(long idParcours, long idEtudiant)
        {
            ArgumentNullException.ThrowIfNull(Context.Etudiants);
            ArgumentNullException.ThrowIfNull(Context.Parcours);

            Etudiant etudiant = (await Context.Etudiants.FindAsync(idEtudiant))!;
            Parcours parcours = (await Context.Parcours.FindAsync(idParcours))!;

            // Ajouter l'étudiant au parcours
            if (!parcours.Inscrits.Contains(etudiant))
            {
                parcours.Inscrits.Add(etudiant);
                await Context.SaveChangesAsync();
            }
        }

        // Méthode pour obtenir un parcours avec ses étudiants
        public async Task<Parcours?> ObtenirParcoursAvecEtudiantsAsync(long idParcours)
        {
            return await Context.Parcours
                .Include(p => p.Inscrits)
                .FirstOrDefaultAsync(p => p.Id == idParcours);
        }
    }