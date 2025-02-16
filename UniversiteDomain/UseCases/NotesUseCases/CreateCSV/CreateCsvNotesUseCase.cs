using CsvHelper;
using CsvHelper.Configuration;
//using Microsoft.Extensions.Localization;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UniversiteDomain.Exceptions.NotesExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCase.CreateCSV
{
    public class CreateCsvNotesUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public CreateCsvNotesUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        // Vérifie si l'utilisateur a les autorisations nécessaires
        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }

        // Exécute la génération du fichier CSV
        public async Task<Stream> ExecuteAsync(long numeroUe)
        {
            await CheckBusinessRules(numeroUe);
            // Récupérer les étudiants et leurs notes pour le numéro de l'UE spécifié
            var etudiants = await _repositoryFactory.EtudiantRepository().FindByConditionAsync(e => e.ParcoursSuivi.UesEnseignees.Any(ue => ue.Id.Equals(numeroUe)));
            var ue = await _repositoryFactory.UeRepository().FindAsync(numeroUe);
            var memoryStream = new MemoryStream();
            var writer = new StreamWriter(memoryStream);
            var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            });

            // Écrire les en-têtes du CSV
            csv.WriteField("IdEtudiant");
            csv.WriteField("NumeroEtudiant");
            csv.WriteField("Nom");
            csv.WriteField("Prenom");
            csv.WriteField("Email");
            csv.WriteField("Note");
            csv.WriteField("IntituleUE");
            csv.NextRecord();
            // Écrire les données des étudiants
            foreach (var etudiant in etudiants)
            {
                csv.WriteField(etudiant.Id);
                csv.WriteField(etudiant.NumEtud);
                csv.WriteField(etudiant.Nom);
                csv.WriteField(etudiant.Prenom);
                csv.WriteField(etudiant.Email);
                //récupère la note d'un étudiant en vérifiant si la note correspond a l'étudiant et à l'Ue
                var note = await _repositoryFactory.NotesRepository().FindByConditionAsync(n => n.EtudiantId.Equals(etudiant.Id) && n.UeId.Equals(numeroUe));
                if(!note.Any())
                    csv.WriteField("Saisir Ici La nouvelle note");
                foreach (var n in note)
                {
                    csv.WriteField(n.EtudiantId);

                }
                csv.WriteField(ue.Intitule);
                csv.NextRecord();
            }
            

            // Revenir au début du flux pour le retourner en réponse
            writer.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        public async Task CheckBusinessRules(long numeroUe)
        {
          

            // Vérifier si l'UE existe
            var ue = await _repositoryFactory.UeRepository().FindByConditionAsync(u => u.Id == numeroUe);
            if (ue == null || !ue.Any())
            {
                throw new UeNotFoundException("L'UE spécifiée n'existe pas.");
            }

            // Vérifier s'il y a des étudiants inscrits dans l'UE
            var etudiants = await _repositoryFactory.EtudiantRepository().FindByConditionAsync(e => e.ParcoursSuivi.UesEnseignees.Any(ue => ue.Id == numeroUe));
            if (etudiants == null || !etudiants.Any())
            {
                throw new UeNotFoundException("Aucun étudiant trouvé pour cette UE.");
            }

            var notes = await _repositoryFactory.NotesRepository().FindByConditionAsync(n => n.UeId == numeroUe);
            if (notes == null || !notes.Any())
            {
                throw new NotesNotFoundException("Aucune note enregistrée pour cette UE.");
            }
        }

    }
}
