using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;

namespace UniversiteDomainUnitTests;

public class EtudiantUnitTest
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task CreateEtudiantUseCase()
    {
        // Préparation des données
        long id = 1;
        string numEtud = "et1";
        string nom = "Durant";
        string prenom = "Jean";
        string email = "jean.durant@etud.u-picardie.fr";

        // Objet d'entrée sans Id
        Etudiant etudiantSansId = new Etudiant { NumEtud = numEtud, Nom = nom, Prenom = prenom, Email = email };

        // Objet attendu après création
        Etudiant etudiantCree = new Etudiant { Id = id, NumEtud = numEtud, Nom = nom, Prenom = prenom, Email = email };

        // Configuration des mocks
        var mockEtudiant = new Mock<IEtudiantRepository>();

        // Simule qu'aucun étudiant avec le même NumEtud n'existe déjà
        mockEtudiant
            .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Etudiant, bool>>>()))
            .ReturnsAsync(new List<Etudiant>()); // Retourne une liste vide

        // Simule la création d'un étudiant et son retour avec un Id
        mockEtudiant
            .Setup(repo => repo.CreateAsync(It.Is<Etudiant>(e =>
                e.NumEtud == etudiantSansId.NumEtud &&
                e.Nom == etudiantSansId.Nom &&
                e.Prenom == etudiantSansId.Prenom &&
                e.Email == etudiantSansId.Email)))
            .ReturnsAsync(etudiantCree);

        // Simule l'enregistrement des changements
        mockEtudiant.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Création du mock de la factory
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory
            .Setup(factory => factory.EtudiantRepository())
            .Returns(mockEtudiant.Object);

        // Création du UseCase
        CreateEtudiantUseCase useCase = new CreateEtudiantUseCase(mockFactory.Object);

        // Appel du UseCase
        Etudiant etudiantTeste = await useCase.ExecuteAsync(etudiantSansId);

        // Vérifications
        Assert.That(etudiantTeste.Id, Is.EqualTo(etudiantCree.Id));
        Assert.That(etudiantTeste.NumEtud, Is.EqualTo(etudiantCree.NumEtud));
        Assert.That(etudiantTeste.Nom, Is.EqualTo(etudiantCree.Nom));
        Assert.That(etudiantTeste.Prenom, Is.EqualTo(etudiantCree.Prenom));
        Assert.That(etudiantTeste.Email, Is.EqualTo(etudiantCree.Email));

        // Vérification des appels aux mocks
        mockEtudiant.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Etudiant, bool>>>()), Times.Once);
        mockEtudiant.Verify(repo => repo.CreateAsync(It.IsAny<Etudiant>()), Times.Once);
        mockEtudiant.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }
}
