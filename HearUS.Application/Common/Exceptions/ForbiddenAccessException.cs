namespace HearUS.Application.Common.Exceptions;

public sealed class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException(string message = "لا تملك صلاحية تنفيذ هذا الإجراء.")
        : base(message)
    {
    }
}
