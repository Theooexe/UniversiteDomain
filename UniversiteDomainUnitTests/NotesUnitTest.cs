using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.NotesUseCases.Create;

namespace UniversiteDomainUnitTests;

public class NotesUnitTest
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public async Task CreateUeUseCase()
    {
        // Préparation des données
        long id = 1;
        float valeur = 15.5f;
        
        // Objet d'entrée sans Id
        Notes notesSansId = new Notes { Valeur = valeur};
        
        // Objet attendu après création
        Notes notesCree = new Notes { Id = id, Valeur = valeur};

        // Configuration des mocks
        var mockNotesRepository = new Mock<INotesRepository>();
        mockNotesRepository
            .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()))
            .ReturnsAsync(new List<Notes>()); // Aucune UE existante trouvée

        mockNotesRepository
            .Setup(repo => repo.CreateAsync(It.Is<Notes>(u => u.Valeur == valeur)))
            .ReturnsAsync(notesCree); // Retourne l'UE créée

        mockNotesRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.CompletedTask); // Simule une sauvegarde réussie

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

        // Vérification des appels aux mocks
        mockNotesRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
        mockNotesRepository.Verify(repo => repo.CreateAsync(It.IsAny<Notes>()), Times.Once);
        mockNotesRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}