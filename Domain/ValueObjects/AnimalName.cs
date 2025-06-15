using System;

namespace Domain.ValueObjects;

public sealed record AnimalName
{
    public string Value { get; private set; }

    // Для EF Core
    private AnimalName() { }

    public AnimalName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Animal name cannot be empty", nameof(value));
        
        Value = value;
    }

    public override string ToString() => Value;
}