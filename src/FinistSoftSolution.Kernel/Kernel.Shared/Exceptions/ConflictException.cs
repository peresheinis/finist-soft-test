namespace Kernel.Shared.Exceptions;

public class ConflictException : AlertingException
{
    private ConflictException(string code, string message, string details, Exception? innerException = null)
        : base(code, message, details, innerException)
    {
    }

    public static ConflictException Conflict(string details)
    {
        return new ConflictException(ExceptionCodes.Conflict, "Ошибка при выполнении запроса", details);
    }

    public static ConflictException NotFound(string details)
    {
        return new ConflictException(ExceptionCodes.EntityNotFound, "Объект не найден", details);
    }

    public static ConflictException AlreadyExist(string details)
    {
        return new ConflictException(ExceptionCodes.EntityAlreadyExistConflict, "Объект уже существует", details);
    }

    public static ConflictException PermissionDenied(string details)
    {
        return new ConflictException(ExceptionCodes.PermissionDenied, "Доступ запрещён", details);
    }

    public static ConflictException SessionExpired(string details)
    {
        return new ConflictException(ExceptionCodes.SessionExpired, "Сессия истекла", details);
    }

    public static ConflictException ServiceUnavailable(string details)
    {
        return new ConflictException(ExceptionCodes.ServiceUnavailable, "Удаленный сервис не доступен", details);
    }
}