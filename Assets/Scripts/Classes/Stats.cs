/// <summary>
/// Base class for all stats
/// </summary>
public abstract class Stats : GameData
{
    public Element mainElement;
    public Element secondElement;
    public int life = 100;
    public int maxLife = 100;
    public int attack = 20;
    public int defense;
    public int eldryz;
    public int maxEldryz;
    public int stamina;
    public int maxStamina;
}