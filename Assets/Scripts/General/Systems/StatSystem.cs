public class StatSystem
{
    //General Stats
    private int HP;

    //Attack Stats
    private int DMG;
    private float CRIT;

    public StatSystem (PlayerStatsSO _stats)
    {
        HP = _stats.HP;
        DMG = _stats.ATK;
        CRIT = _stats.CRIT;
    }

    public StatSystem (EnemyStatsSO _stats)
    {
        HP = _stats.HP;
        DMG = _stats.ATK;
        CRIT = _stats.CRIT;
    }

    public int GetHP (){return HP;}
    public float GetCRIT(){return CRIT;}
    public int GetDMG(){return DMG;}
}
