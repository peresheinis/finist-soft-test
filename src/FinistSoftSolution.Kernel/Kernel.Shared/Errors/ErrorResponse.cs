namespace Kernel.Shared.Errors;

public class ErrorResponse
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string Message { get; set; }
    /// <summary>
    /// Детали ошибки
    /// </summary>
    public string Details { get; set; }
    /// <summary>
    /// Идентификатор запроса
    /// </summary>
    public string RequestId { get; set; }
}
