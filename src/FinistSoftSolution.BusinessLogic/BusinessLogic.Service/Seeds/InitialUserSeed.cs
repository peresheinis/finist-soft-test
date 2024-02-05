using BusinessLogic.Core;
using BusinessLogic.Core.Entities;
using BusinessLogic.Core.Repositories;
using BusinessLogic.Core.ValueObjects;
using BusinessLogic.Service.Configurations;
using Microsoft.Extensions.Options;

namespace BusinessLogic.Infrastructure.Seeds;

internal sealed class InitialUserSeed : SeedBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUsersRepository _usersRepository;
    private readonly IBankAccountsRepository _bankAccountsRepository;
    private readonly InitialUserSettings _initialUserSettings;
    public InitialUserSeed(
        IUsersRepository usersRepository, 
        IBankAccountsRepository bankAccountsRepository,
        IUnitOfWork unitOfWork, IOptions<InitialUserSettings> initialUserSettings)
    {
        if (initialUserSettings.Value is null)
        {
            throw new ArgumentNullException(nameof(initialUserSettings));
        }

        _unitOfWork = unitOfWork;
        _usersRepository = usersRepository;
        _initialUserSettings = initialUserSettings.Value;
        _bankAccountsRepository = bankAccountsRepository;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var user = await _usersRepository.GetByPhoneAsync(_initialUserSettings.PhoneNumber);

        if (user is null)
        {
            user = await User.CreateAsync(
                _initialUserSettings.PhoneNumber,
                _initialUserSettings.Password,
                new FullName(
                    _initialUserSettings.FirstName,
                    _initialUserSettings.LastName,
                    _initialUserSettings.MiddleName),
                IsPhoneNumberUnique);

            var cardNumber = CreateAccountNumber(); 
            var urgentNumber = CreateAccountNumber();
            var onDemandNumber = CreateAccountNumber();

            var cardAccount = await BankAccount
                .CreateAsync(BankAccountType.Card, cardNumber, IsBankAccountUnique);

            var onDemandAccount = await BankAccount
                .CreateAsync(BankAccountType.OnDemand, onDemandNumber, IsBankAccountUnique);

            var urgentAccount = await BankAccount
                .CreateAsync(BankAccountType.Urgent, urgentNumber, IsBankAccountUnique);

            user.AddBankAccount(cardAccount);
            user.AddBankAccount(onDemandAccount);
            user.AddBankAccount(urgentAccount);

            await _usersRepository.AddAsync(user, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Валидация на уникальность номера телефона
    /// </summary>
    /// <param name="phoneNumber"></param>
    /// <returns></returns>
    private async Task<bool> IsPhoneNumberUnique(string phoneNumber)
    {
        // Тут можно реализовать дополнительный метод
        // в репозитории IsUniqueByPhone<bool>(phoneNumber: string)

        var user = await _usersRepository.GetByPhoneAsync(phoneNumber);

        return user is null;
    }

    /// <summary>
    /// Валидация на уникальность номера карты
    /// </summary>
    /// <param name="bankAccountNumber"></param>
    /// <returns></returns>
    private async Task<bool> IsBankAccountUnique(AccountNumber bankAccountNumber)
    {
        // Тут можно реализовать метод
        // в репозитории IsUniqueByNumber<bool>(bankAccountNumber: string) вместо текущего

        var account = await _bankAccountsRepository.GetByNumberAsync(bankAccountNumber.Value);

        return account is null;
    }

    /// <summary>
    /// Зарандомить номер карты пользователя
    /// </summary>
    /// <returns></returns>
    private AccountNumber CreateAccountNumber() => // Так делать не хорошо, рандомить не нужно,
                                                   // но так как это для теста, то думаю, что сойдёт
        new AccountNumber(
            $"{Random.Shared.Next(1000, 9999)}" +
            $"{Random.Shared.Next(1000, 9999)}" +
            $"{Random.Shared.Next(1000, 9999)}" +
            $"{Random.Shared.Next(1000, 9999)}" +
            $"{Random.Shared.Next(1000, 9999)}");
}
