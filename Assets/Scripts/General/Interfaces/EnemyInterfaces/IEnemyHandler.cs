using UnityEngine;

public interface IEnemyHandler
{
    public abstract void Initialize();
    public abstract void TakeDamage(int _d, bool _isCrit);
}
