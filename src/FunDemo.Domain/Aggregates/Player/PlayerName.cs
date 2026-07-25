namespace FunDemo.Domain.Aggregates.Player;

public class PlayerName : ValueObject<string>
{
    const int MinLength = 3;
    const int MaxLength = 10;

    public PlayerName(string value) : base(value)
    {
        if (value.Length is < 3 or > 10)
        {
            throw new ArgumentException($"Player name must be between {MinLength} and {MaxLength} characters.");
        }
    }
}