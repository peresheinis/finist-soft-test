using BusinessLogic.Core.Entities;

namespace BusinessLogic.Core.Repositories;

/// <summary>
/// Репозиторий для записи данных в БД
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IWriteRepositoryBase<TEntity> 
    where TEntity : EntityBase
{
    /// <summary>
    /// Добавить сущность
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновить сущность
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public void Update(TEntity entity);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    /// <param name="entity"></param>
    public void Remove(TEntity entity);
}