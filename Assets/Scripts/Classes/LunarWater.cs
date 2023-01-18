using System;

[Serializable]
public class LunarWater : IItem
{
    public int hpRecovery;

    public LunarWater() { }

    public LunarWater(int hpRecovery)
    {
        this.hpRecovery = hpRecovery;
    }

    public void Use()
    {
        Player.stats.life = Player.stats.life + hpRecovery <= Player.stats.maxLife ?
            Player.stats.life + hpRecovery : Player.stats.maxLife;
    }
}