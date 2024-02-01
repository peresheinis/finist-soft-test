using BusinessLogic.Core;

namespace BusinessLogic.Infrastructure;

public class UnitOfWorkPostgres : IUnitOfWork
{
    private readonly DatabaseContext _databaseContext;
    public UnitOfWorkPostgres(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _databaseContext.SaveChangesAsync(cancellationToken);
    }
}