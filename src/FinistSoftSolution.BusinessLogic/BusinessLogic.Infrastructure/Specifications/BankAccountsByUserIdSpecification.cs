using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class BankAccountsByUserIdSpecification : SpecificationBase<BankAccount>
{
    public BankAccountsByUserIdSpecification(Guid userId)
        : base(bankAccount => bankAccount.UserId.Equals(userId))
    {
    }
}