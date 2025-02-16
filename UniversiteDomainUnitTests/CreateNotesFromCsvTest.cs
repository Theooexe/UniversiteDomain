using Moq;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NoteUseCases.Create;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniversiteDomain.DataAdapters;

namespace UniversiteDomainUnitTests
{
    public class CreateNotesFromCsvTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task CreateNotesFromCsvTestexec()
        {
            // Arrange
            var mockRepositoryFactory = new Mock<IRepositoryFactory>();
            var mockEtudiantRepo = new Mock<IEtudiantRepository>();
            var mockUeRepo = new Mock<IUeRepository>();
            var mockNotesRepo = new Mock<INotesRepository>();
            var mockParcoursRepo = new Mock<IParcoursRepository>();

            mockRepositoryFactory.Setup(repo => repo.EtudiantRepository()).Returns(mockEtudiantRepo.Object);
            mockRepositoryFactory.Setup(repo => repo.UeRepository()).Returns(mockUeRepo.Object);
            mockRepositoryFactory.Setup(repo => repo.NotesRepository()).Returns(mockNotesRepo.Object);
            mockRepositoryFactory.Setup(repo => repo.ParcoursRepository()).Returns(mockParcoursRepo.Object);

            var useCase = new CreateNotesFromCsvUseCase(mockRepositoryFactory.Object);

            string csvContent = 
                "IdEtudiant,NumeroEtudiant,Nom,Prenom,Email,Note,IntituleUE\n1,123456,Durand,Paul,paul.durand@example.com,15,Mathématiques\n2,789012,Martin,Sophie,sophie.martin@example.com,,Physique\n"; 

            byte[] csvBytes = System.Text.Encoding.UTF8.GetBytes(csvContent);

            var etudiants = new List<Etudiant>
            {
                new Etudiant { Id = 1,Nom = "Durand", Prenom = "Paul", Email = "paul.durand@example.com" },
                new Etudiant { Id = 2,Nom = "Martin", Prenom = "Sophie", Email = "sophie.martin@example.com" }
            };

            var ue = new Ue { Id = 101, Intitule = "Mathématiques" };
            var note = new Notes { EtudiantId = 1, UeId = 101, Valeur = 15 };

            // Mock les retours des repository
            mockEtudiantRepo.Setup(repo => repo.FindByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Etudiant, bool>>>()))
                .ReturnsAsync(etudiants);

            mockUeRepo.Setup(repo => repo.FindAsync(101)).ReturnsAsync(ue);

            mockNotesRepo.Setup(repo => repo.FindByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Notes, bool>>>()))
                .ReturnsAsync(new List<Notes> { note }); // L'étudiant 1 a déjà une note

            mockParcoursRepo.Setup(repo => repo.FindByConditionAsync(It.IsAny<System.Linq.Expressions.Expression<System.Func<Parcours, bool>>>()))
                .ReturnsAsync(new List<Parcours> { new Parcours() }); // Étudiant inscrit à l'UE

            // Act
            await useCase.ExecuteAsync(101, csvBytes);

            // Assert
            mockRepositoryFactory.Verify(repo => repo.NotesRepository().AddNoteAsync(101, 1, 15), Times.Once);
            mockRepositoryFactory.Verify(repo => repo.NotesRepository().AddNoteAsync(101, 2, 0), Times.Once); // Note vide => 0 par défaut
        }
    }
}
