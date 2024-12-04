using System.Linq.Expressions;
 using Moq;
 using UniversiteDomain.DataAdapters;
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
     public async Task CreateEtudiantUseCase()
     {
         long id = 1;
         String numeroUe = "1";
         string intitule="ASI";
         
         Ue ueSansId = new Ue{NumeroUe=numeroUe,Intitule=intitule};
         var mock = new Mock<IUeRepository>();
         var reponseFindByCondition = new List<Ue>();
        
         mock.Setup(repo=>repo.FindByConditionAsync(It.IsAny<Expression<Func<Ue, bool>>>())).ReturnsAsync(reponseFindByCondition);
         
         // Simulation de la fonction Create
         // On lui dit que l'ajout d'un étudiant renvoie un étudiant avec l'Id 1
         Ue ueCree =new Ue{Id=id,NumeroUe=numeroUe};
         mock.Setup(repoUe=>repoUe.CreateAsync(ueSansId)).ReturnsAsync(ueCree);
         
         // On crée le bouchon (un faux etudiantRepository). Il est prêt à être utilisé
         var fauxueRepository = mock.Object;
         
         // Création du use case en injectant notre faux repository
         CreateUeUseCase useCase=new CreateUeUseCase(fauxueRepository);
         // Appel du use case
         var ueTeste=await useCase.ExecuteAsync(ueSansId);
         
         // Vérification du résultat
         Assert.That(ueTeste.Id, Is.EqualTo(ueCree.Id));
         Assert.That(ueTeste.NumeroUe, Is.EqualTo(ueCree.NumeroUe));
         Assert.That(ueTeste.Intitule, Is.EqualTo(ueCree.Intitule));
         
     }
 }