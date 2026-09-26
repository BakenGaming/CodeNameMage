using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(EnemyHandler _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyChaseBaseInstance.DoEnterStateLogic();
    }
    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyChaseBaseInstance.DoExitStateLogic();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(enemy.isAttacking) enemy.stateMachine.ChangeState(enemy.attackState);
        
        enemy.enemyChaseBaseInstance.DoFrameUpdateLogic();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.enemyChaseBaseInstance.DoPhysicsUpdateLogic();
    }
    public override void CollisionHandler()
    {
        base.CollisionHandler();
        enemy.enemyChaseBaseInstance.DoCollisionHandlerLogic();
    }
    public override void CheckForPlayerDeath()
    {
        base.CheckForPlayerDeath();
        enemy.enemyChaseBaseInstance.DoCheckForPlayerDeathLogic();
    }
    public override void AnimationTriggerEvent(EnemyHandler.AnimationTriggerType _animationTriggerType){}
}
