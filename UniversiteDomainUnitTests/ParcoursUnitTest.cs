﻿using System.Linq.Expressions;
using Moq;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.AddEtudiantDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.AddEtudiantDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.AddUeDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;

namespace UniversiteDomainUnitTests;

public class ParcoursUnitTest
{
       
    [Test]
    public async Task AddEtudiantDansParcoursUseCase()
    {
        long idEtudiant = 1;
        long idParcours = 3;
        Etudiant etudiant= new Etudiant { Id = 1, NumEtud = "1", Nom = "nom1", Prenom = "prenom1", Email = "1" };
        Parcours parcours = new Parcours{Id=3, NomParcours = "Ue 3", AnneeFormation = 1};
        
        // On initialise une fausse datasource qui va simuler un EtudiantRepository
        var mockEtudiant = new Mock<IEtudiantRepository>();
        var mockParcours = new Mock<IParcoursRepository>();
        List<Etudiant> etudiants = new List<Etudiant>();
        etudiants.Add(new Etudiant{Id=1});
        mockEtudiant
            .Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idEtudiant)))
            .ReturnsAsync(etudiants);

        List<Parcours> parcourses = new List<Parcours>();
        parcourses.Add(parcours);
        
        List<Parcours> parcoursFinaux = new List<Parcours>();
        Parcours parcoursFinal = parcours;
        parcoursFinal.Inscrits.Add(etudiant);
        parcoursFinaux.Add(parcours);
        
        mockParcours
            .Setup(repo=>repo.FindByConditionAsync(e=>e.Id.Equals(idParcours)))
            .ReturnsAsync(parcourses);
        mockParcours
            .Setup(repo => repo.AddEtudiantAsync(idParcours, idEtudiant))
            .ReturnsAsync(parcoursFinal);
        // Création du use case en utilisant le mock comme datasource
        AddEtudiantDansParcoursUseCase useCase=new AddEtudiantDansParcoursUseCase(mockEtudiant.Object, mockParcours.Object);
        
        // Appel du use case
        var parcoursTest=await useCase.ExecuteAsync(idParcours, idEtudiant);
        // Vérification du résultat
        Assert.That(parcoursTest.Id, Is.EqualTo(parcoursFinal.Id));
        Assert.That(parcoursTest.Inscrits, Is.Not.Null);
        Assert.That(parcoursTest.Inscrits.Count, Is.EqualTo(1));
        Assert.That(parcoursTest.Inscrits[0].Id, Is.EqualTo(idEtudiant));
    }
    
    public async Task CreateParcoursUseCase()
    {
        long id = 1;
        string nomParcours = "parcours1";
        int anneeFormation = 1;
        
        // On crée l'étudiant qui doit être ajouté en base
        Parcours parcoursSansId = new Parcours{NomParcours= nomParcours, AnneeFormation = anneeFormation};
        //  Créons le mock du repository
        // On initialise une fausse datasource qui va simuler un EtudiantRepository
        var mock = new Mock<IParcoursRepository>();
        // Il faut ensuite aller dans le use case pour voir quelles fonctions simuler
        // Nous devons simuler FindByCondition et Create
        
        // Simulation de la fonction FindByCondition
        // On dit à ce mock que l'étudiant n'existe pas déjà
        // La réponse à l'appel FindByCondition est donc une liste vide
        var reponseFindByCondition = new List<Parcours>();
        // On crée un bouchon dans le mock pour la fonction FindByCondition
        // Quelque soit le paramètre de la fonction FindByCondition, on renvoie la liste vide
        mock.Setup(repo=>repo.FindByConditionAsync(It.IsAny<Expression<Func<Parcours, bool>>>())).ReturnsAsync(reponseFindByCondition);
        
        // Simulation de la fonction Create
        // On lui dit que l'ajout d'un étudiant renvoie un étudiant avec l'Id 1
        Parcours parcoursCree =new Parcours{Id=id,NomParcours= nomParcours, AnneeFormation = anneeFormation};
        mock.Setup(repoParcours=>repoParcours.CreateAsync(parcoursSansId)).ReturnsAsync(parcoursCree);
        
        // On crée le bouchon (un faux etudiantRepository). Il est prêt à être utilisé
        var fauxParcoursRepository = mock.Object;
        
        // Création du use case en injectant notre faux repository
        CreateParcoursUseCase useCase=new CreateParcoursUseCase(fauxParcoursRepository);
        // Appel du use case
        var parcoursTest=await useCase.ExecuteAsync(parcoursSansId);
        
        // Vérification du résultat
        Assert.That(parcoursTest.Id, Is.EqualTo(parcoursCree.Id));
        Assert.That(parcoursTest.NomParcours, Is.EqualTo(parcoursCree.NomParcours));
        Assert.That(parcoursTest.AnneeFormation, Is.EqualTo(parcoursCree.AnneeFormation));
    }
}