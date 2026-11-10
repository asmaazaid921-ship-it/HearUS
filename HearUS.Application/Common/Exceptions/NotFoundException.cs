namespace HearUS.Application.Common.Exceptions;

public sealed class NotFoundException : Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }

    public NotFoundException(string entityName, object key)
        : base($"لم يتم العثور على {entityName} بالمعرف {key}.")
    {
    }
}
