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
        
        public async Task CreateNoteUseCase()
        {
          
            long etudiantId = 1;
            long ueId = 1;
            float valeur = 15;

            Notes notesSansId = new Notes { Valeur = valeur, EtudiantId = etudiantId, UeId = ueId };
            Notes notesCree = new Notes { Valeur = valeur, EtudiantId = etudiantId, UeId = ueId };

           
            var mockNotesRepository = new Mock<INotesRepository>();

           
            mockNotesRepository
                .Setup(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()))
                .ReturnsAsync(new List<Notes>());

          
            mockNotesRepository
                .Setup(repo => repo.CreateAsync(It.Is<Notes>(n => n.Valeur == valeur && n.EtudiantId == etudiantId && n.UeId == ueId)))
                .ReturnsAsync(notesCree);

           
            mockNotesRepository
                .Setup(repo => repo.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var mockFactory = new Mock<IRepositoryFactory>();
            mockFactory
                .Setup(factory => factory.NotesRepository())
                .Returns(mockNotesRepository.Object);
            mockFactory
                .Setup(factory => factory.SaveChangesAsync())
                .Returns(Task.CompletedTask);

          
            CreateNotesUseCase useCase = new CreateNotesUseCase(mockFactory.Object);
            Notes notesTestee = await useCase.ExecuteAsync(notesSansId);

            
            Assert.That(notesTestee.Valeur, Is.EqualTo(notesCree.Valeur));
            Assert.That(notesTestee.EtudiantId, Is.EqualTo(etudiantId));
            Assert.That(notesTestee.UeId, Is.EqualTo(ueId));
            
            mockNotesRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.CreateAsync(It.IsAny<Notes>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task CreateNoteDejaExistante()
        {
           
            long etudiantId = 1;
            long ueId = 1;
            float valeur = 15;
            
            
            Notes notesSansId = new Notes { Valeur = valeur, EtudiantId = etudiantId, UeId = ueId};
            var mockNotesRepository = new Mock<INotesRepository>();

            
            mockNotesRepository
                .Setup(repo => repo.FindByConditionAsync(It.Is<Expression<Func<Notes, bool>>>(expr =>
                    expr.Compile()(new Notes { EtudiantId = etudiantId, UeId = ueId }) == true))) 
                .ReturnsAsync(new List<Notes> { new Notes { EtudiantId = etudiantId, UeId = ueId } }); 

            
            var mockFactory = new Mock<IRepositoryFactory>();
            mockFactory
                .Setup(factory => factory.NotesRepository())
                .Returns(mockNotesRepository.Object);

          
            CreateNotesUseCase useCase = new CreateNotesUseCase(mockFactory.Object);
            var exception = Assert.ThrowsAsync<NoteDejaExistanteException>(async () =>
                await useCase.ExecuteAsync(notesSansId)
            );
            
            Assert.That(exception.Message, Is.EqualTo("L'étudiant a déjà une note pour cette UE."));
            
            mockNotesRepository.Verify(repo => repo.FindByConditionAsync(It.IsAny<Expression<Func<Notes, bool>>>()), Times.Once);
            mockNotesRepository.Verify(repo => repo.CreateAsync(It.IsAny<Notes>()), Times.Never); 
            mockNotesRepository.Verify(repo => repo.SaveChangesAsync(), Times.Never); 
        }
    }
}
