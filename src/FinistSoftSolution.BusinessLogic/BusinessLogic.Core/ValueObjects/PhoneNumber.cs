using System.Text.RegularExpressions;

namespace BusinessLogic.Core.ValueObjects;

/// <summary>
/// ValueObject - номер телефона
/// </summary>
public class PhoneNumber : IEquatable<PhoneNumber>
{
    /// <summary>
    /// RegexPattern для валидации номера телефона
    /// </summary>
    public const string RegexPattern = @"^(7|8)?\d{10}$"; // Использовал такую валидацию, что бы не усложнять и не тратить время

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string Number { get; }

    public PhoneNumber(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        { 
            throw new ArgumentNullException(nameof(number), "Phone number cannot be null or empty");
        }

        if (!Regex.IsMatch(number, RegexPattern))
        {
            throw new ArgumentException("Phone number is not valid.");
        }

        Number = number;
    }

    public override bool Equals(object? obj)
    {
        if (obj is PhoneNumber numberValue)
        {
            return Equals(numberValue);
        }

        if (obj is string numberString)
        { 
            return Equals(numberString);
        }

        return false;
    }

    public bool Equals(PhoneNumber? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Number, other.Number, StringComparison.OrdinalIgnoreCase);
    }

    public bool Equals(string? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Number, other, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Number);
    }

    public override string ToString()
    {
        return Number;
    }
}
