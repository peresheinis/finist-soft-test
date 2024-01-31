using BusinessLogic.Core.Entities;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public class BankAccountsRepository : RepositoryBase<BankAccount>, IBankAccountsRepository
{
    public BankAccountsRepository(DatabaseContext databaseContext) : base(databaseContext.BankAccounts)
    {
    }

    public async Task AddAsync(BankAccount entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    // Не стал запариваться с PagedList<BankAccount>, лучше конечно возвращать PagedList<BankAccount>
    // { Items: IReadOnlyCollection<BankAccount>, TotalItems: CountAsync() - посчитать количество всех записей в БД (Необходимо для пагинации на фронтенде) },
    // но так как у меня в тз ничего не сказано про то что банковских счетов может быть миллион,
    // а на картинке я увидел 3 штуки, решил оставить так
    public async Task<IReadOnlyCollection<BankAccount>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        await ApplySpecification(new BankAccountsByUserIdSpecification(userId))
            .ToListAsync(cancellationToken); 

    public Task<BankAccount?> GetByIdAsync(Guid key, CancellationToken cancellationToken = default) =>
        ApplySpecification(new BankAccountsByIdSpecification(key))
           .SingleOrDefaultAsync(cancellationToken);

    public void Remove(BankAccount entity) =>
        DbSet.Remove(entity);

    public void Update(BankAccount entity) =>
        DbSet.Update(entity);

    public Task<BankAccount?> GetByNumberAsync(string number, CancellationToken cancellationToken = default) =>
        ApplySpecification(new BankAccountsByNumberSpecification(number))
            .SingleOrDefaultAsync(cancellationToken);
}
