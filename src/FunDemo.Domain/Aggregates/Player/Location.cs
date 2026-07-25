namespace FunDemo.Domain.Aggregates.Player;

public class Location(int x, int y)
{
    public static Location HumanStartLocation => new Location(0, 0);

    public int X { get; set; } = x;
    public int Y { get; set; } = y;
}