namespace UniversiteDomain.Exceptions.NotesExceptions;

[Serializable]
public class NoteDejaExistanteException : Exception
{
    public  NoteDejaExistanteException() : base() { }
    
    public  NoteDejaExistanteException(string message) : base(message) { }
    
    public  NoteDejaExistanteException(string message, Exception inner) : base(message, inner) { }
}