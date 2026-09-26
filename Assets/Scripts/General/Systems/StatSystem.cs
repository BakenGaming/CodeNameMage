public class StatSystem
{
        //General Stats
    private int HP;
    private float SPEED;
    
    //Player Stats
    private float DASHPOWER;
    private float DASHCD;
    private float DASHTIME;

    //Attack Stats
    private int DMG;
    private float CRIT;

    public StatSystem (PlayerStatsSO _stats)
    {
        HP = _stats.HP;
        SPEED = _stats.SPEED;
        DMG = _stats.ATK;
        CRIT = _stats.CRIT;
    }

    public StatSystem (EnemyStatsSO _stats)
    {
        HP = _stats.HP;
        SPEED = _stats.SPEED;
        DMG = _stats.ATK;
        CRIT = _stats.CRIT;
    }

    public int GetHP (){return HP;}
    public float GetSPEED(){return SPEED;}
    public float GetDASHPOWER(){return DASHPOWER;}
    public float GetDASHCD(){return DASHCD;}
    public float GetDASHTIME(){return DASHTIME;}
    public float GetCRIT(){return CRIT;}
    public int GetDMG(){return DMG;}
}
