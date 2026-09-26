using UnityEngine;

public class EnemyState
{
    protected EnemyHandler enemy;
    protected EnemyStateMachine enemyStateMachine;

    public EnemyState(EnemyHandler _enemy, EnemyStateMachine _enemyStateMachine)
    {
        enemy = _enemy;
        enemyStateMachine = _enemyStateMachine;
    }

    public virtual void EnterState(){}
    public virtual void ExitState(){}
    public virtual void FrameUpdate(){}
    public virtual void PhysicsUpdate(){}
    public virtual void CollisionHandler(){}
    public virtual void CheckForPlayerDeath(){}
    public virtual void AnimationTriggerEvent(EnemyHandler.AnimationTriggerType _animationTriggerType){}

    
}
