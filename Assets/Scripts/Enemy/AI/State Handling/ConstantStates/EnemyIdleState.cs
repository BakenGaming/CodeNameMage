using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(EnemyHandler _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyIdleBaseInstance.DoEnterStateLogic();
    }
    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyIdleBaseInstance.DoExitStateLogic();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if(enemy.isAggroed) enemy.stateMachine.ChangeState(enemy.chaseState);

        enemy.enemyIdleBaseInstance.DoFrameUpdateLogic();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.enemyIdleBaseInstance.DoPhysicsUpdateLogic();
    }
    public override void CollisionHandler()
    {
        base.CollisionHandler();
        enemy.enemyIdleBaseInstance.DoCollisionHandlerLogic();
    }
    public override void AnimationTriggerEvent(EnemyHandler.AnimationTriggerType _animationTriggerType){}

}
