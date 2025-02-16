using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.Exceptions.NotesExceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using UniversiteDomain.Dtos;
using UniversiteDomain.Exceptions.EtudiantExceptions;
using UniversiteDomain.Exceptions.UeExceptions;

namespace UniversiteDomain.UseCases.NoteUseCases.Create
{
    public class CreateNotesFromCsvUseCase
    {
        private readonly IRepositoryFactory _repositoryFactory;

        public CreateNotesFromCsvUseCase(IRepositoryFactory repositoryFactory)
        {
            _repositoryFactory = repositoryFactory;
        }

        // Vérifie si l'utilisateur est autorisé à ajouter des notes
        public bool IsAuthorized(string role)
        {
            return role.Equals(Roles.Responsable) || role.Equals(Roles.Scolarite);
        }

        // Cette méthode est exécutée pour ajouter les notes à partir du fichier CSV
        public async Task ExecuteAsync(long numeroUe, byte[] csvFile)
        {
            // Convertir le tableau de bytes en stream
            using var memoryStream = new MemoryStream(csvFile);
            using var reader = new StreamReader(memoryStream);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null
            });

            // Enregistrer le ClassMap avec le TypeConverter personnalisé
            csv.Context.RegisterClassMap<NotesCsvDtoMap>();

            var records = csv.GetRecords<NotesCsvDto>().ToList();  // Lecture du CSV dans une liste de DTO
            if (records.Count == 0)
            {
                throw new Exception("Le fichier CSV est vide.");
            }

            foreach (var record in records)
            {
                await ProcessNoteAsync(record, numeroUe);
            }
        }

        // Traitement pour chaque enregistrement de note
        private async Task ProcessNoteAsync(NotesCsvDto record, long numeroUe)
        {
            await CheckBusinessRules(record, numeroUe);
            
            var etudiant = await _repositoryFactory.EtudiantRepository().FindAsync(record.IdEtudiant);
            if (etudiant == null)
            {
                throw new EtudiantNotFoundException($"Étudiant avec le numéro {record.IdEtudiant} non trouvé.");
            }
            // Recherche de l'UE par son numéro
            var ue = await _repositoryFactory.UeRepository().FindAsync(numeroUe);
            if (ue == null)
            {
                throw new UeNotFoundException($"UE avec le numéro {numeroUe} non trouvée.");
            }
            // Vérification si une note existe déjà pour cet étudiant et cette UE
            var existingNote = await _repositoryFactory.NotesRepository().FindAsync(etudiant.Id, ue.Id);
            if (existingNote != null)
            {
                Console.WriteLine($"Note existante trouvée pour l'étudiant {etudiant.Id} dans l'UE {ue.Id}. Aucune nouvelle note ajoutée.");
                return; // Ne rien ajouter si la note existe déjà
            }
            var note = new Notes
            {
                UeId = ue.Id,
                EtudiantId = etudiant.Id,
                Valeur = record.Note,
            };
            // Ajout de la note dans le système
            await _repositoryFactory.NotesRepository().AddNoteAsync(ue.Id,etudiant.Id,note.Valeur);
        }
        public async Task CheckBusinessRules(NotesCsvDto record , long numeroUe) {
            var etudiant = await _repositoryFactory.EtudiantRepository().FindAsync(record.IdEtudiant);
            if (etudiant == null)
            {
                throw new EtudiantNotFoundException($"Étudiant avec le numéro {record.IdEtudiant} non trouvé.");
            }
            // Recherche de l'UE par son numéro
            var ue = await _repositoryFactory.UeRepository().FindAsync(numeroUe);
            if (ue == null)
            {
                throw new UeNotFoundException($"UE avec le numéro {numeroUe} non trouvée.");
            }
            // Vérification si l'étudiant suit l'UE (parcours associé)
            var parcours = await _repositoryFactory.ParcoursRepository()
                .FindByConditionAsync(p => p.Inscrits.Any(inscri => inscri.Id == etudiant.Id) && p.UesEnseignees.Any(ens => ens.Id == ue.Id));
            
            if (parcours == null || parcours.Count == 0)
            {
                throw new Exception($"L'étudiant ne suit pas l'UE {numeroUe}.");
            }
        }
    }
}
