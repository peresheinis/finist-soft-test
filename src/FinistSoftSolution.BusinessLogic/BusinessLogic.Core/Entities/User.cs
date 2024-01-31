using BusinessLogic.Core.ValueObjects;

namespace BusinessLogic.Core.Entities;

public class User : EntityBase<Guid>
{
    private readonly List<BankAccount>? _bankAccounts;
    private User(
        FullName fullName,
        PhoneNumber phoneNumber,
        string password)
    {
        FullName = fullName;
        PhoneNumber = phoneNumber;
        Password = password;
    }

    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    public FullName FullName { get; private set; }

    /// <summary>
    /// Номер телефона пользователя
    /// </summary>
    public PhoneNumber PhoneNumber { get; private set; }

    /// <summary>
    /// Пароль, без Hash и без Salt
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// Счета клиента
    /// </summary>
    public IReadOnlyCollection<BankAccount>? BankAccounts { get; private set; }

    /// <summary>
    /// Добавить банковский счёт пользователю
    /// </summary>
    /// <param name="bankAccount"></param>
    public void AddBankAccount(BankAccount bankAccount)
    {
        if (_bankAccounts is null)
        {
            throw new InvalidOperationException($"Need to _.Include(_ => _.{BankAccounts})");
        }

        _bankAccounts.Add(bankAccount);
    }

    /// <summary>
    /// Удалить банковский счёт клиента
    /// </summary>
    /// <param name="bankAccount"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void RemoveBankAccount(BankAccount bankAccount)
    {
        if (_bankAccounts is null)
        {
            throw new InvalidOperationException($"Need to _.Include(_ => _.{BankAccounts})");
        }

        _bankAccounts.Remove(bankAccount);
    }

    /// <summary>
    /// Установить имя пользователя
    /// </summary>
    /// <param name="fullName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void SetFullName(FullName fullName)
    {
        FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
    }

    /// <summary>
    /// Установить номер телефона
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <param name="isPhoneNumberUniqueValidation"></param>
    public async Task SetPhoneNumberAsync(PhoneNumber phoneNumber, Func<PhoneNumber, Task<bool>> isPhoneNumberUniqueValidation)
    {
        if (await isPhoneNumberUniqueValidation(phoneNumber))
        {
            throw new InvalidOperationException("Phone number is not unique.");
        }

        PhoneNumber = phoneNumber;
    }

    public static async Task<User> CreateAsync(PhoneNumber phoneNumber, FullName fullName, string password, Func<PhoneNumber, Task<bool>> isPhoneNumberUniqueValidation)
    {
        if (phoneNumber is null)
        {
            throw new ArgumentNullException(nameof(phoneNumber), $"{nameof(phoneNumber)} cannot be null or empty.");
        }

        if (fullName is null)
        {
            throw new ArgumentNullException(nameof(fullName), $"{nameof(fullName)} cannot be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(password)) 
        {
            throw new ArgumentNullException(password, $"{nameof(password)} cannot be null or empty.");
        }

        if (!await isPhoneNumberUniqueValidation(phoneNumber))
        {
            throw new InvalidOperationException($"{phoneNumber} is not unique!");
        }

        return new User(fullName, phoneNumber, password);
    }
}