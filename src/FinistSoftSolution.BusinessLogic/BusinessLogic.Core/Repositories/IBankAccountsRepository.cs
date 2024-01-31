using BusinessLogic.Core.Entities;

namespace BusinessLogic.Core.Repositories;

/// <summary>
/// Интерфейс для работы с банковскими счетами
/// </summary>
public interface IBankAccountsRepository : IBankAccountsWriteRepository, IBankAccountsReadRepository
{ 
}

/// <summary>
/// Интерфейс для чтения банковских аккаунтов из БД
/// </summary>
public interface IBankAccountsReadRepository : IReadRepositoryBase<BankAccount, Guid>
{
    /// <summary>
    /// Получить список банковских счетов пользователя
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<IReadOnlyCollection<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Интерфейс для записи банковских аккаунтов в БД
/// </summary>
public interface IBankAccountsWriteRepository : IWriteRepositoryBase<BankAccount>
{ 
}