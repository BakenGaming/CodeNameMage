using UnityEngine;

public class EnemyIdleSOBase : ScriptableObject
{
    protected EnemyHandler enemy;
    protected Transform transform;
    protected GameObject gameObject;
    protected Transform playerTransform;

    public virtual void Initialize(GameObject _gameObject, EnemyHandler _enemy, Transform _playerTransform)
    {
        gameObject = _gameObject;
        transform = gameObject.transform;
        enemy = _enemy;
        playerTransform = _playerTransform;
    }

    public virtual void DoEnterStateLogic() {}
    public virtual void DoExitStateLogic(){ResetValues();}
    public virtual void DoFrameUpdateLogic(){}
    public virtual void DoPhysicsUpdateLogic(){}
    public virtual void DoCollisionHandlerLogic(){}
    public virtual void DoAnimationTriggerEventLogic(EnemyHandler.AnimationTriggerType _animationTriggerType){}
    public virtual void ResetValues(){}

}
