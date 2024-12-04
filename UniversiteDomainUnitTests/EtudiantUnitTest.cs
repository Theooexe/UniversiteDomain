using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases;
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
        long id = 1;
        String numEtud = "et1";
        string nom = "Durant";
        string prenom = "Jean";
        string email = "jean.durant@etud.u-picardie.fr";
        
        Etudiant etudiantSansId = new Etudiant{NumEtud=numEtud, Nom = nom, Prenom=prenom, Email=email};
        var mockEtudiant = new Mock<IEtudiantRepository>();
       
        var reponseFindByCondition = new List<Etudiant>();
        mockEtudiant.Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(numEtud))).ReturnsAsync((List<Etudiant>)null);
        
       
        Etudiant etudiantCree =new Etudiant{Id=id,NumEtud=numEtud, Nom = nom, Prenom=prenom, Email=email};
        mockEtudiant.Setup(repoEtudiant=>repoEtudiant.CreateAsync(etudiantSansId)).ReturnsAsync(etudiantCree);
        
        var mockFactory = new Mock<IRepositoryFactory>();
        mockFactory.Setup(facto => facto.EtudiantRepository()).Returns(mockEtudiant.Object);

        
        CreateEtudiantUseCase useCase=new CreateEtudiantUseCase(mockFactory.Object);
        var etudiantTeste=await useCase.ExecuteAsync(etudiantSansId);
        
       
        Assert.That(etudiantTeste.Id, Is.EqualTo(etudiantCree.Id));
        Assert.That(etudiantTeste.NumEtud, Is.EqualTo(etudiantCree.NumEtud));
        Assert.That(etudiantTeste.Nom, Is.EqualTo(etudiantCree.Nom));
        Assert.That(etudiantTeste.Prenom, Is.EqualTo(etudiantCree.Prenom));
        Assert.That(etudiantTeste.Email, Is.EqualTo(etudiantCree.Email));
    }

}