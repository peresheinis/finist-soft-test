using BusinessLogic.Core.ValueObjects;
using Kernel.Shared.Exceptions;

namespace BusinessLogic.Core.Entities;

public class User : EntityBase<Guid>
{
    private readonly List<BankAccount>? _bankAccounts;

    private User() { }

    private User(
        FullName fullName,
        string phoneNumber,
        string password)
    {
        _bankAccounts = new List<BankAccount>();

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
    public string PhoneNumber { get; private set; }

    /// <summary>
    /// Пароль, без Hash и без Salt
    /// </summary>
    public string Password { get; private set; }

    /// <summary>
    /// Счета клиента
    /// </summary>
    public IReadOnlyCollection<BankAccount>? BankAccounts => _bankAccounts;

    /// <summary>
    /// Добавить банковский счёт пользователю
    /// </summary>
    /// <param name="bankAccount"></param>
    /// <exception cref="InvalidOperationException"></exception>
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
    /// <exception cref="ConflictException"></exception>
    public async Task SetPhoneNumberAsync(string phoneNumber, Func<string, Task<bool>> isPhoneNumberUniqueValidation)
    {
        if (await isPhoneNumberUniqueValidation(phoneNumber))
        {
            throw ConflictException.AlreadyExist($"This phone number is already in use.");
        }

        PhoneNumber = phoneNumber;
    }

    /// <summary>
    /// Создать пользователя
    /// </summary>
    /// <param name="phoneNumber">Номер телефона</param>
    /// <param name="password">Пароль</param>
    /// <param name="fullName">Полное имя пользователя</param>
    /// <param name="isPhoneNumberUniqueValidation">Проверка на уникальность номера телефона</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ConflictException"></exception>
    public static async Task<User> CreateAsync(string phoneNumber, string password, FullName fullName, Func<string, Task<bool>> isPhoneNumberUniqueValidation)
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
            throw ConflictException.AlreadyExist($"This phone number is already in use.");
        }

        return new User(fullName, phoneNumber, password);
    }
}