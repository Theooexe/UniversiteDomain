using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NotesUseCases.Create;
using UniversiteDomain.Exceptions.NotesExceptions;

namespace UniversiteDomainUnitTests
{
    public class NotesUnitTest
    {
        [SetUp]
        public void Setup()
        {
        }
        
        [Test]
        
        public async Task CreateNoteUseCase_ValidCase()
        {
            // Préparation des données
            long etudiantId = 1;
            long ueId = 1;
            float valeur = 15.5f;

            // Objet d'entrée sans Id
            Notes notesSansId = new Notes { Valeur = valeur, EtudiantId = etudiantId, UeId = ueId};

            // Objet attendu après création
            Notes notesCree = new Notes { Id = 1, Valeur = valeur, EtudiantId = etudiantId, UeId = ueId};

            // Configuration des mocks
            var mockNotesRepository = new Mock<INotesRepository>();

            // Configuration pour simuler l'absence de note existante pour cet étudiant et cette UE
            mockNotesRepository
                .Setup(repo => repo.FindByConditionAsync(It.Is<Expression<Func<Notes, bool>>>(expr =>
                    expr.Compile()(new Notes { EtudiantId = etudiantId, UeId = ueId }) == false))) // Simule l'absence de note
                .ReturnsAsync(new List<Notes>()); // Retourne une liste vide, pas null

            // Configuration pour simuler la création de la note
            mockNotesRepository
                .Setup(repo => repo.CreateAsync(It.Is<Notes>(n => n.Valeur == valeur && n.EtudiantId == etudiantId && n.UeId == ueId)))
                .ReturnsAsync(notesCree); // Retourne la note créée

            // Simuler la sauvegarde réussie
            mockNotesRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            // Mock pour IRepositoryFactory
            var mockFactory = new Mock<IRepositoryFactory>();
            mockFactory
                .Setup(factory => factory.NotesRepository())
                .Returns(mockNotesRepository.Object);

            // Création du UseCase
            CreateNotesUseCase useCase = new CreateNotesUseCase(mockFactory.Object);

            // Appel du UseCase
            Notes notesTestee = await useCase.ExecuteAsync(notesSansId);

            // Vérifications
            Assert.That(notesTestee.Id, Is.EqualTo(notesCree.Id));
            Assert.That(notesTestee.Valeur, Is.EqualTo(notesCree.Valeur));
            Assert.That(notesTestee.EtudiantId, Is.EqualTo(etudiantId));
            Assert.That(notesTestee.UeId, Is.EqualTo(ueId));

            // Vérification des appels aux mocks
            mockNotesRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.CreateAsync(It.IsAny<Notes>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreateNoteUseCase_AlreadyExistingNote_ShouldThrowNoteDejaExistanteException()
        {
            // Préparation des données
            long etudiantId = 1;
            long ueId = 1;
            float valeur = 15.5f;
            
            // Objet d'entrée sans Id
            Notes notesSansId = new Notes { Valeur = valeur, EtudiantId = etudiantId, UeId = ueId};

            // Configuration des mocks
            var mockNotesRepository = new Mock<INotesRepository>();

            // Configuration pour simuler une note existante pour cet étudiant et cette UE
            mockNotesRepository
                .Setup(repo => repo.FindByConditionAsync(It.Is<Expression<Func<Notes, bool>>>(expr =>
                    expr.Compile()(new Notes { EtudiantId = etudiantId, UeId = ueId }) == true))) // Simule la présence d'une note existante
                .ReturnsAsync(new List<Notes> { new Notes { EtudiantId = etudiantId, UeId = ueId } }); // Une note existe déjà pour cet étudiant et cette UE

            // Mock pour IRepositoryFactory
            var mockFactory = new Mock<IRepositoryFactory>();
            mockFactory
                .Setup(factory => factory.NotesRepository())
                .Returns(mockNotesRepository.Object);

            // Création du UseCase
            CreateNotesUseCase useCase = new CreateNotesUseCase(mockFactory.Object);

            // Appel du UseCase et vérification de l'exception
            var exception = Assert.ThrowsAsync<NoteDejaExistanteException>(async () =>
                await useCase.ExecuteAsync(notesSansId)
            );

            // Vérification du message d'erreur
            Assert.That(exception.Message, Is.EqualTo("L'étudiant a déjà une note pour cette UE."));

            // Vérification des appels aux mocks
            mockNotesRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.CreateAsync(It.IsAny<Notes>()), Times.Never); // La création ne doit pas avoir été appelée
            mockNotesRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never); // La sauvegarde ne doit pas avoir été appelée
        }
    }
}
