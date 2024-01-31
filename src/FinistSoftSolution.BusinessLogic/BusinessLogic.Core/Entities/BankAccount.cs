using BusinessLogic.Core.ValueObjects;

namespace BusinessLogic.Core.Entities;

/// <summary>
/// Банковский счёт пользователя
/// </summary>
public class BankAccount : EntityBase<Guid>
{
    /// <summary>
    /// Банковский счёт пользователя
    /// </summary>
    /// <param name="bankAccountType"></param>
    /// <param name="accountNumber"></param>
    private BankAccount(BankAccountType bankAccountType, AccountNumber accountNumber)
    { 
        AccountType = bankAccountType;
        AccountNumber = accountNumber;
    }

    /// <summary>
    /// Идентификатор пользователя, которому принадлежит счёт
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Тип счёта пользователя: Срочный / Карточный / До востребования
    /// </summary>
    public BankAccountType AccountType { get; private set; }

    /// <summary>
    /// Номер банковского счёта
    /// </summary>
    public AccountNumber AccountNumber { get; private set; }

    /// <summary>
    /// Создать банковский счёт
    /// </summary>
    /// <param name="bankAccountType">Тип банковского счёта</param>
    /// <param name="accountNumber">Номер счёта</param>
    /// <param name="isAccountNumberUniqueValidation">Валидация на уникальность банковского счёта</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<BankAccount> CreateAsync(BankAccountType bankAccountType, AccountNumber accountNumber, Func<AccountNumber, Task<bool>> isAccountNumberUniqueValidation)
    {
        if (accountNumber is null)
        { 
            throw new ArgumentNullException(nameof(accountNumber), $"{accountNumber} cannot be null!");
        }

        if (await isAccountNumberUniqueValidation(accountNumber))
        {
            throw new InvalidOperationException("Account number is not unique!");
        }

        return new BankAccount(bankAccountType, accountNumber);
    }
}

public enum BankAccountType
{
    /// <summary>
    /// Неотложный / срочный
    /// </summary>
    Urgent,
    /// <summary>
    /// До востребования
    /// </summary>
    OnDemand,
    /// <summary>
    /// Карточный счёт
    /// </summary>
    Card
}
