using BusinessLogic.Core.Entities;

namespace BusinessLogic.Core.Repositories;

/// <summary>
/// Интерфейс для работы с пользователями из БД
/// </summary>
public interface IUsersRepository : IUsersWriteRepository, IUsersReadRepository
{
}

/// <summary>
/// Интерфейс для чтения пользователей из БД
/// </summary>
public interface IUsersReadRepository : IReadRepositoryBase<User, Guid>
{
    /// <summary>
    /// Получить пользователя по номеру телефона
    /// </summary>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<User?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default);
}

/// <summary>
/// Интерфейс для записи пользователей в БД
/// </summary>
public interface IUsersWriteRepository : IWriteRepositoryBase<User>
{ 
}