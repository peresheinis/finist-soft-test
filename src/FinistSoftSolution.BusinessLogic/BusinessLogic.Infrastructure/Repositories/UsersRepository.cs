using BusinessLogic.Core.Entities;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;

namespace BusinessLogic.Infrastructure.Repositories;

/// <summary>
/// Репозиторий для работы с пользователями
/// </summary>
public class UsersRepository : RepositoryBase<User>, IUsersRepository
{
    public UsersRepository(DatabaseContext databaseContext) : base(databaseContext.Users)
    {
    }

    public async Task AddAsync(User entity, CancellationToken cancellationToken = default) =>
        await DbSet.AddAsync(entity, cancellationToken);

    public Task<User?> GetByIdAsync(Guid key, CancellationToken cancellationToken = default) =>
        ApplySpecification(new UserByIdSpecification(key))
            .SingleOrDefaultAsync(cancellationToken);

    public Task<User?> GetByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default) =>
        ApplySpecification(new UserByPhoneSpecification(phoneNumber))
            .SingleOrDefaultAsync(cancellationToken);

    public void Remove(User entity) =>
        DbSet.Remove(entity);

    public void Update(User entity) =>
        DbSet.Update(entity);
}