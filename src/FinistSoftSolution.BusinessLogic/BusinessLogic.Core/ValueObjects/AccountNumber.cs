using System.Text.RegularExpressions;

namespace BusinessLogic.Core.ValueObjects;

/// <summary>
/// ValueObject - номер счёта пользователя
/// </summary>
public class AccountNumber : IEquatable<AccountNumber>
{
    /// <summary>
    /// Regex паттерн для валидации номера карты
    /// </summary>
    public const string RegexPattern = @"^\d{20}$";
    /// <summary>
    /// Номер карты
    /// </summary>
    public string Value { get; }
    /// <summary>
    /// Номер карты
    /// </summary>
    /// <param name="value">Значение номера карты</param>
    /// <exception cref="ArgumentException"></exception>
    public AccountNumber(string value)
    {
        if (!Regex.IsMatch(value, RegexPattern))
        {
            throw new ArgumentException("Invalid Account Number");
        }

        Value = value;
    }

    public override bool Equals(object? obj)
    {
        if (obj is AccountNumber otherAccount)
        {
            return Equals(otherAccount);
        }

        return false;
    }

    public bool Equals(AccountNumber? other)
    {
        if (other == null)
        {
            return false;
        }

        return string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public override string ToString()
    {
        return Value;
    }
}