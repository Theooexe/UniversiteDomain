namespace UniversiteDomain.DataAdapters.DataAdaptersFactory;

public interface IRepositoryFactory
{
    IParcoursRepository ParcoursRepository();
    IEtudiantRepository EtudiantRepository();
    IUeRepository UeRepository();
    INotesRepository NotesRepository();
    IUniversiteRoleRepository UniversiteRoleRepository();
    IUniversiteUserRepository UniversiteUserRepository();
    Task EnsureDeletedAsync();
    Task EnsureCreatedAsync();
    Task SaveChangesAsync();
}