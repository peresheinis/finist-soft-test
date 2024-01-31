using BusinessLogic.Core.Entities;

namespace BusinessLogic.Core.Repositories;

/// <summary>
/// Репозиторий для чтения данных из БД
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IReadRepositoryBase<TEntity, TKey> 
    where TEntity : EntityBase
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Получить сущность по идентификатору
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Task<TEntity?> GetByIdAsync(TKey key, CancellationToken cancellationToken = default);
}