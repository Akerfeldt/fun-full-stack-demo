namespace FunDemo.Domain.Aggregates;

public abstract class ValueObject<T>(T value)
{
    public T Value { get; protected set; } = value;
}