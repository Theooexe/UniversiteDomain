﻿using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteDomain.Entities;
//using UniversiteDomain.Entities.SecurityEntities;
using UniversiteDomain.UseCases.EtudiantUseCases.Create;
using UniversiteDomain.UseCases.NotesUseCases;
using UniversiteDomain.UseCases.NotesUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.Create;
using UniversiteDomain.UseCases.ParcoursUseCases.AddEtudiantDansParcours;
using UniversiteDomain.UseCases.ParcoursUseCases.AddUeDansParcours;
using UniversiteDomain.UseCases.SecurityUseCases.Create;
//using UniversiteDomain.UseCases.SecurityUseCases;
using UniversiteDomain.UseCases.UeUseCases.Create;

namespace UniversiteDomain.JeuxDeDonnees;

public class BasicBdBuilder(IRepositoryFactory repositoryFactory) : BdBuilder(repositoryFactory)
{
    private readonly string Password = "Miage2025#";

    private readonly Etudiant[] _etudiants =
    [
        new Etudiant { Id=1,NumEtud = "03BDKZ65", Nom = "Dupont", Prenom = "Antoine", Email = "antoine.dupont@etud.u-picardie.fr" },
        new Etudiant { Id=2,NumEtud = "JEIZ03JZ", Nom = "Ntamak", Prenom = "Romain", Email = "roman.ntamak@etud.u-picardie.fr" },
        new Etudiant { Id=3,NumEtud = "62830483", Nom = "Barassi", Prenom = "Pierre-Louis", Email = "pierre-louis.barassi@etud.u-picardie.fr" },
        new Etudiant { Id=4,NumEtud = "J6HZK922", Nom = "Jelong", Prenom = "Antony", Email = "antony.jelong@etud.u-picardie.fr" },
        new Etudiant { Id=5,NumEtud = "PAD89345", Nom = "Akki", Prenom = "Pita", Email = "pita.akki@etud.u-picardie.fr" },
        new Etudiant { Id=6,NumEtud = "RG8647FF", Nom = "Mauvaka", Prenom = "Peato", Email = "peato.mauvaka@etud.u-picardie.fr" }
    ];
    private struct UserNonEtudiant
    {
        public string UserName;
        public string Email;
        public string Role;
    }
    private readonly UserNonEtudiant[] _usersNonEtudiants =
    [
        new UserNonEtudiant { UserName = "anne.lapujade@u-picardie.fr", Email = "anne.lapujade@u-picardie.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "plouisberquez@gmail.com", Email = "plouisberquez@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "jabin.julian.univ@gmail.com", Email = "jabin.julian.univ@gmail.com", Role = "Responsable" },
        new UserNonEtudiant { UserName = "mehdy.chk@outlook.fr", Email = "mehdy.chk@outlook.fr", Role = "Responsable" },
        new UserNonEtudiant { UserName = "stephanie.dertin@u-picardie.fr", Email = "stephanie.dertin@u-picardie.fr", Role = "Scolarite" }
    ];

    private readonly Parcours[] _parcours =
    [
        new Parcours { Id=1,NomParcours = "M1", AnneeFormation = 1 },
        new Parcours { Id=2,NomParcours = "OSIE", AnneeFormation = 2 },
        new Parcours { Id=3,NomParcours = "ITD", AnneeFormation = 2 },
        new Parcours { Id=4,NomParcours = "IDD", AnneeFormation = 2 }
    ];

    private readonly Ue[] _ues =
    [
        new Ue { Id=1, NumeroUe = "ISI_01", Intitule = "Architecture des SI 1" },
        new Ue { Id=2, NumeroUe = "ISI_02", Intitule = "Conduite de projet" },
        new Ue { Id=3, NumeroUe = "GEO_05", Intitule = "Marketing" },
        new Ue { Id=4, NumeroUe = "INFO_18", Intitule = "Architecture des SI 2" }
    ];

    private struct Inscription
    {
        public long EtudiantId;
        public long ParcoursId;
    }

    private readonly Inscription[] _inscriptions =
    [
        // EtudiantId, ParcoursId
        new Inscription { EtudiantId = 1, ParcoursId = 2 },
        new Inscription { EtudiantId = 2, ParcoursId = 1 },
        new Inscription { EtudiantId = 3, ParcoursId = 1 },
        new Inscription { EtudiantId = 4, ParcoursId = 1 },
        new Inscription { EtudiantId = 5, ParcoursId = 3 },
        new Inscription { EtudiantId = 6, ParcoursId = 4 }
    ];

    private struct UeDansParcours
    {
        public long UeId;
        public long ParcoursId;
    }

    private readonly UeDansParcours[] _maquette =
    [
        new UeDansParcours { UeId = 1, ParcoursId = 1 },
        new UeDansParcours { UeId = 2, ParcoursId = 1 },
        new UeDansParcours { UeId = 3, ParcoursId = 1 },
        new UeDansParcours { UeId = 4, ParcoursId = 2 },
        new UeDansParcours { UeId = 4, ParcoursId = 3 },
        new UeDansParcours { UeId = 4, ParcoursId = 4 }
    ];
    
    private struct Notes
    {
        public long EtudiantId;
        public long UeId;
        public float Valeur;
    }
    
    private readonly Notes[] _notes =
    [
        new Notes {EtudiantId = 2,UeId = 1, Valeur = 12 },
        new Notes {EtudiantId = 3,UeId = 1, Valeur = (float)8.5 },
        new Notes {EtudiantId = 4,UeId = 1, Valeur = 16 },
        new Notes {EtudiantId = 2,UeId = 2, Valeur = 14 },
        new Notes {EtudiantId = 3,UeId = 2, Valeur = 6 },
        new Notes {EtudiantId = 4,UeId = 3, Valeur = (float)11.5 },
        new Notes {EtudiantId = 1,UeId = 4, Valeur = 10 },
        new Notes {EtudiantId = 5,UeId = 4, Valeur = (float)18.3 },
        new Notes {EtudiantId = 6,UeId = 4, Valeur = 12 }
    ];
    protected override async Task RegenererBdAsync()
    {
        // Ici je décide de supprimer et recréer la BD
        await repositoryFactory.EnsureDeletedAsync();
        await repositoryFactory.EnsureCreatedAsync();
    }
    protected override async Task BuildEtudiantsAsync()
    {
        foreach (Etudiant e in _etudiants)
        {
            await new CreateEtudiantUseCase(repositoryFactory).ExecuteAsync(e);
        }
    }
    protected override async Task BuildParcoursAsync()
    {
        foreach (Parcours parcours in _parcours)
        {
            await new CreateParcoursUseCase(repositoryFactory).ExecuteAsync(parcours);
        }
    }
    protected override async Task BuildUesAsync()
    {
        foreach (Ue ue in _ues)
        {
            await new CreateUeUseCase(repositoryFactory).ExecuteAsync(ue);
        }
    }

    protected override async Task InscrireEtudiantsAsync()
    {
        foreach (Inscription i in _inscriptions)
        {
            await new AddEtudiantDansParcoursUseCase(repositoryFactory).ExecuteAsync(i.ParcoursId,i.EtudiantId);
        }
    }
    protected override async Task BuildMaquetteAsync()
    {
        foreach(UeDansParcours u in _maquette)
        {
            await new AddUeDansParcoursUseCase(repositoryFactory).ExecuteAsync(u.ParcoursId, u.UeId);
        }
    }

    protected override async Task NoterAsync()
    {
        foreach( var notes in _notes)
        {
            await new CreateNotesUseCase(repositoryFactory).ExecuteAsync(notes.Valeur, notes.EtudiantId, notes.UeId);
        }
    }
    
    protected override async Task BuildRolesAsync()
    {
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Responsable);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Scolarite);
        await new CreateUniversiteRoleUseCase(repositoryFactory).ExecuteAsync(Roles.Etudiant);
        
    }

    protected override async Task BuildUsersAsync()
    {
			
        CreateUniversiteUserUseCase uc = new CreateUniversiteUserUseCase(repositoryFactory);
        // Création des étudiants
        foreach (var etudiant in _etudiants)
        {
            await uc.ExecuteAsync(etudiant.Email, etudiant.Email, this.Password, Roles.Etudiant,etudiant);
        }
        
        // Création des responsbles
        foreach (var user in _usersNonEtudiants)
        {
            await uc.ExecuteAsync(user.Email, user.Email, this.Password, user.Role, null);
        }
        
    }
}