using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class BankAccountsByIdSpecification : SpecificationBase<BankAccount>
{
    public BankAccountsByIdSpecification(Guid accountId)
        : base(bankAccount => bankAccount.Id.Equals(accountId))
    {
    }
}