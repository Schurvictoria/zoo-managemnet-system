using System;

namespace Domain.ValueObjects;

public sealed record FoodType
{
    public string Value { get; private set; }

    // Для EF Core
    private FoodType() { }

    public FoodType(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Food type cannot be empty", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value;
}