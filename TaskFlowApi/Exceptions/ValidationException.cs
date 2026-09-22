namespace TaskFlowApi.Exceptions;

public class ValidationException : Exception
{
    public string Field { get; }
    public List<string> Errors { get; }

    public ValidationException(string message, string field, List<string>? errors = null) 
        : base(message)
    {
        Field = field;
        Errors = errors ?? [message];
    }
}