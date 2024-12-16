namespace SuperHero.Service.Infra.Exceptions;

public sealed class ForbidException : BaseException
{
    public ForbidException()
        : base()
    {
    }
    public ForbidException(string message)
        : base(message)
    {
    }
    public ForbidException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}