﻿namespace Kernel.Shared.Exceptions;

/// <summary>Базовый класс ошибок приложения, зависящих от пользователя</summary>
public abstract class AlertingException : Exception
{
    protected AlertingException(string code, string message, string details, Exception? innerException = null)
        : base(message, innerException)
    {
        if (string.IsNullOrEmpty(code))
            throw new ArgumentNullException(nameof(code));

        Code = code;
        Details = details;
    }

    public string Code { get; }
    public string Details { get; }
}
