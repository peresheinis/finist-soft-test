namespace BusinessLogic.Core;

public interface IUnitOfWork
{
    /// <summary>
    /// Сохранить изменения в бд
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}