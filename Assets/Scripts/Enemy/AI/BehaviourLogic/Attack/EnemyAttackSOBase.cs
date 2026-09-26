using UnityEngine;

public class EnemyAttackSOBase : ScriptableObject
{
    protected EnemyHandler enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;
    protected EnemyStatsSO stats;

    public virtual void Initialize(GameObject _gameObject, EnemyHandler _enemy, Transform _playerTransform, EnemyStatsSO _stats)
    {
        gameObject = _gameObject;
        transform = gameObject.transform;
        enemy = _enemy;
        playerTransform = _playerTransform;
        stats = _stats;
    }

    public virtual void DoEnterStateLogic(){}
    public virtual void DoExitStateLogic(){ResetValues();}
    public virtual void DoFrameUpdateLogic(){}
    public virtual void DoPhysicsUpdateLogic(){}
    public virtual void DoCollisionHandlerLogic(){}
    public virtual void DoAnimationTriggerEventLogic(EnemyHandler.AnimationTriggerType _animationTriggerType){}
    public virtual void ResetValues(){}
}
