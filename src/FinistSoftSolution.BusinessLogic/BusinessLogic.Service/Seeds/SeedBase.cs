namespace BusinessLogic.Infrastructure.Seeds;

/// <summary>
/// Базовый класс для заполнения данными бд
/// </summary>
public abstract class SeedBase
{
    /// <summary>
    /// Выполнить задачу по засеяванию сущностей
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public abstract Task ExecuteAsync(CancellationToken cancellationToken = default);
}
