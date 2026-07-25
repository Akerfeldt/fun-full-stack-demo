namespace FunDemo.Domain.Aggregates.Player;

public class Player
{
    public Player(
        UserId userId, 
        PlayerName name, 
        PlayerClass playerClass, 
        Race race)
    {
        UserId = userId;
        Name = name;
        Class = playerClass;
        Race = race;

        Location = race switch
        {
            Race.Human => Location.HumanStartLocation,
            _ => throw new NotImplementedException(),
        };
    }

    public Player(
        UserId userId, 
        PlayerName name, 
        PlayerClass playerClass, 
        Race race, 
        Location location)
    {
        UserId = userId;
        Name = name;
        Class = playerClass;
        Race = race;
        Location = location;
    }

    public UserId UserId { get; init; }
    public PlayerName Name { get; init; }
    public Location Location { get; init; }
    public PlayerClass Class { get; init; }
    public Race Race { get; init; }

    public void GoUp()
    {
        Location.Y++;
    }

    public void GoDown()
    {
        Location.Y--;
    }

    public void GoLeft()
    {
        Location.X--;
    }

    public void GoRight()
    {
        Location.X++;
    }
}
