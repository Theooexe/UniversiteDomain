using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomainUnitTests;

public class UeUnitTest
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
        string numeroUe = "1";
        string intitule = "ASI";
        
        // Objet d'entrée sans Id
        Ue ueSansId = new Ue { NumeroUe = numeroUe, Intitule = intitule };
        
        // Objet attendu après création
        Ue ueCree = new Ue { Id = id, NumeroUe = numeroUe, Intitule = intitule };

        // Configuration des mocks
        var mockUeRepository = new Mock<IUeRepository>();
        mockUeRepository
            .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()))
            .ReturnsAsync(new List<Ue>()); // Aucune UE existante trouvée

        mockUeRepository
            .Setup(repo => repo.CreateAsync(It.Is<Ue>(u => u.NumeroUe == numeroUe && u.Intitule == intitule)))
            .ReturnsAsync(ueCree); // Retourne l'UE créée

        mockUeRepository
            .Setup(repo => repo.SaveChangesAsync())
            .Returns(Task.CompletedTask); // Simule une sauvegarde réussie

        // Mock pour IRepositoryFactory
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory
            .Setup(factory => factory.UeRepository())
            .Returns(mockUeRepository.Object);

        // Création du UseCase
        CreateUeUseCase useCase = new CreateUeUseCase(mockFactory.Object);

        // Appel du UseCase
        Ue ueTestee = await useCase.ExecuteAsync(ueSansId);

        // Vérifications
        Assert.That(ueTestee.Id, Is.EqualTo(ueCree.Id));
        Assert.That(ueTestee.NumeroUe, Is.EqualTo(ueCree.NumeroUe));
        Assert.That(ueTestee.Intitule, Is.EqualTo(ueCree.Intitule));

        // Vérification des appels aux mocks
        mockUeRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>()), Times.Once);
        mockUeRepository.Verify(repo => repo.CreateAsync(It.IsAny<Ue>()), Times.Once);
        mockUeRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
