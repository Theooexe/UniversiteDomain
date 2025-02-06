namespace UniversiteDomain.DataAdapters.DataAdaptersFactory;

public interface IRepositoryFactory
{
    IParcoursRepository ParcoursRepository();
    IEtudiantRepository EtudiantRepository();
    IUeRepository UeRepository();
    IUniversiteRoleRepository UniversiteRoleRepository();
    IUniversiteUserRepository UniversiteUserRepository();
    
    INotesRepository NotesRepository();
    Task EnsureDeletedAsync();
    Task EnsureCreatedAsync();
    Task SaveChangesAsync();
}