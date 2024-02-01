namespace BusinessLogic.Core.ValueObjects;

/// <summary>
/// ValueObject - полное имя пользователя
/// </summary>
public class FullName : IEquatable<FullName>
{
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string FirstName { get; private set; }

    /// <summary>
    /// Отчество пользователя
    /// </summary>
    public string? MiddleName { get; private set; }

    /// <summary>
    /// Фамилия пользователя
    /// </summary>
    public string LastName { get; private set; }

    /// <summary>
    /// Полное имя пользователя
    /// </summary>
    /// <param name="firstName">Имя пользователя</param>
    /// <param name="lastName">Фамилия пользователя</param>
    /// <param name="middleName">Отчество пользователя, может быть <c>null</c></param>
    /// <exception cref="ArgumentNullException">Если Имя или Фамилия пользователя пустая строка или null</exception>
    public FullName(string firstName, string lastName, string? middleName = null)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        { 
            throw new ArgumentNullException(nameof(firstName), $"{nameof(firstName)} cannot be null or empty!");
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentNullException(nameof(lastName), $"{nameof(lastName)} cannot be null or empty!");
        }

        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }

    public override bool Equals(object? obj)
    {
        if (obj is FullName otherName)
        {
            return Equals(otherName);
        }

        return false;
    }

    public bool Equals(FullName? other)
    {
        if (other is null)
        {
            return false;
        }

        return 
            string.Equals(LastName, other.LastName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(FirstName, other.FirstName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(MiddleName, other.MiddleName, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        if (string.IsNullOrWhiteSpace(MiddleName))
        {
            return 
               StringComparer.OrdinalIgnoreCase.GetHashCode(FirstName) ^
               StringComparer.OrdinalIgnoreCase.GetHashCode(LastName);
        }

        return
            StringComparer.OrdinalIgnoreCase.GetHashCode(FirstName) ^
            StringComparer.OrdinalIgnoreCase.GetHashCode(LastName) ^
            StringComparer.OrdinalIgnoreCase.GetHashCode(MiddleName);
    }

    public override string ToString()
    {
        if (string.IsNullOrWhiteSpace(MiddleName))
        {
            return $"{FirstName} {LastName}";
        }

        return $"{FirstName} {MiddleName} {LastName}";
    }
}
