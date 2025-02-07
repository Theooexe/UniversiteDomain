using Microsoft.AspNetCore.Identity;
using UniversiteDomain.DataAdapters;
using UniversiteDomain.DataAdapters.DataAdaptersFactory;
using UniversiteEFDataProvider.Data;
using UniversiteEFDataProvider.Entities;
using UniversiteEFDataProvider.Repositories;

namespace UniversiteEFDataProvider.RepositoryFactories;

public class RepositoryFactory(
    UniversiteDbContext context,
    RoleManager<UniversiteRole> roleManager,
    UserManager<UniversiteUser> userManager
) : IRepositoryFactory
{
    private readonly UniversiteDbContext _context = context;
    private readonly RoleManager<UniversiteRole> _roleManager = roleManager;
    private readonly UserManager<UniversiteUser> _userManager = userManager;

    private IParcoursRepository? _parcours;
    private IEtudiantRepository? _etudiants;
    private IUeRepository? _ues;
    private INotesRepository? _notes;
    private IUniversiteRoleRepository? _universiteRole;
    private IUniversiteUserRepository? _universiteUser;

    public IParcoursRepository ParcoursRepository()
    {
        return _parcours ??= new ParcoursRepository(_context);
    }

    public IEtudiantRepository EtudiantRepository()
    {
        return _etudiants ??= new EtudiantRepository(_context);
    }

    public IUeRepository UeRepository()
    {
        return _ues ??= new UeRepository(_context);
    }

    public INotesRepository NotesRepository()
    {
        return _notes ??= new NotesRepository(_context);
    }

    public IUniversiteRoleRepository UniversiteRoleRepository()
    {
        return _universiteRole ??= new UniversiteRoleRepository(_context, _roleManager);
    }

    public IUniversiteUserRepository UniversiteUserRepository()
    {
        return _universiteUser ??= new UniversiteUserRepository(_context, _userManager, _roleManager);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task EnsureCreatedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task EnsureDeletedAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}
