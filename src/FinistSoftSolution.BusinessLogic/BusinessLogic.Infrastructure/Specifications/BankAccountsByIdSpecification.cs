using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class BankAccountsByIdSpecification : SpecificationBase<BankAccount>
{
    public BankAccountsByIdSpecification(Guid accountId)
        : base(bankAccount => bankAccount.Id.Equals(accountId))
    {
    }
}

internal sealed class BankAccountsByNumberSpecification : SpecificationBase<BankAccount>
{
    public BankAccountsByNumberSpecification(string number)
        : base(bankAccount => bankAccount.AccountNumber.Equals(number))
    {
    }
}