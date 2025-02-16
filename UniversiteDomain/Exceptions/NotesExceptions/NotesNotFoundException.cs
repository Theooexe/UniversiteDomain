namespace UniversiteDomain.Exceptions.NotesExceptions;

[Serializable]
public class NotesNotFoundException : Exception
{
    public NotesNotFoundException() : base() { }
    
    public NotesNotFoundException(string message) : base(message) { }
    
    public NotesNotFoundException(string message, Exception inner) : base(message, inner) { }
}