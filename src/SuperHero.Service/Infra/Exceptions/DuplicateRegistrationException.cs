namespace SuperHero.Service.Infra.Exceptions;

[Serializable]
public sealed class DuplicateRegistrationException : BaseException
{
    public DuplicateRegistrationException()
        : base()
    {
    }
    public DuplicateRegistrationException(string message)
        : base(message)
    {
    }
    public DuplicateRegistrationException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}