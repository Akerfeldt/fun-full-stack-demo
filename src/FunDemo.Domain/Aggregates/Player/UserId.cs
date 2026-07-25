namespace FunDemo.Domain.Aggregates.Player;

public class UserId : ValueObject<string>
{
    public UserId(string value) : base(value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentNullException(nameof(value));
        }
    }
}