namespace BusinessLogic.Core.Entities;

/// <summary>
/// Базовая сщуность
/// </summary>
public class EntityBase
{
}

/// <summary>
/// Базовая сущность с идентификатором
/// </summary>
/// <typeparam name="TKey"></typeparam>
public class EntityBase<TKey> : EntityBase where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public TKey Id { get; set; }
}
