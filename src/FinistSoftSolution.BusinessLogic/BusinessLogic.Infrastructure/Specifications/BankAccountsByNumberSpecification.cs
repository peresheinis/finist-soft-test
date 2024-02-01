using BusinessLogic.Core.Entities;

namespace BusinessLogic.Infrastructure.Specifications;

internal sealed class BankAccountsByNumberSpecification : SpecificationBase<BankAccount>
{
    public BankAccountsByNumberSpecification(string number)
        : base(bankAccount => bankAccount.AccountNumber.Equals(number))
    {
    }
}