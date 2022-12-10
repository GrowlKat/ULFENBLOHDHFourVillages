using System;
/// <summary>
/// Contains information about quests and a singleton
/// </summary>
[Serializable]
public class Quest
{
    public bool shohtzsfenCompleted = false;
    public bool ballaghanCompleted = false;
    public bool ayotzorCompleted = false;
    public bool eilighvindCompleted = false;
    public bool currentlyInBattle = false;
    public int enemyQuantity;

    public static Quest singleton = new();
}
