using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using System.Linq.Expressions;

namespace UniversiteDomain.UseCases.EtudiantUseCases.Delete
{
    public class DeleteEtudiantUseCase
    {
        private readonly IRepositoryFactory _factory;
        public DeleteEtudiantUseCase(IRepositoryFactory factory)
        {
            _factory = factory;
        }

        // Première méthode pour supprimer un étudiant en fonction de plusieurs critères
        public async Task<Etudiant> ExecuteAsync(string numEtud, string nom, string prenom, string email)
        {
            var etudiantRepo = _factory.EtudiantRepository();

            // Recherche de l'étudiant par ses critères (numEtud, nom, prénom, email)
            List<Etudiant> etudiants = await etudiantRepo.FindByConditionAsync(
                e => e.NumEtud == numEtud && e.Nom == nom && e.Prenom == prenom && e.Email == email);

            // Si aucun étudiant n'est trouvé, on lève une exception
            if (etudiants == null || etudiants.Count == 0)
            {
                throw new Exception("Étudiant introuvable avec les critères donnés.");
            }

            // On prend le premier étudiant trouvé (il est supposé unique dans ce contexte)
            Etudiant etudiantToDelete = etudiants.First();

            // Supprime l'étudiant
            await etudiantRepo.DeleteAsync(etudiantToDelete);
            await _factory.SaveChangesAsync();

            // Retourne l'étudiant supprimé
            return etudiantToDelete;
        }


        public async Task ExecuteAsync(long numEtud)
        {
            throw new NotImplementedException();
        }
    }
}
