namespace UniversiteDomain.Exceptions.NotesExceptions;

[Serializable]

public class ValeurIncorrecteException : Exception
{
    public ValeurIncorrecteException() : base() { }
    
    public ValeurIncorrecteException(string message) : base(message) { }
    
    public ValeurIncorrecteException(string message, Exception inner) : base(message, inner) { }
}