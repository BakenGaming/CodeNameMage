using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(EnemyHandler _enemy, EnemyStateMachine _enemyStateMachine) : base(_enemy, _enemyStateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyAttackBaseInstance.DoEnterStateLogic();
    }
    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyAttackBaseInstance.DoExitStateLogic();
    }
    public override void FrameUpdate()
    {
        base.FrameUpdate();
        
        if(enemy.isAggroed) enemy.stateMachine.ChangeState(enemy.chaseState);

        enemy.enemyAttackBaseInstance.DoFrameUpdateLogic();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.enemyAttackBaseInstance.DoPhysicsUpdateLogic();
    }
    public override void CollisionHandler()
    {
        base.CollisionHandler();
        enemy.enemyAttackBaseInstance.DoCollisionHandlerLogic();
    }
    public override void CheckForPlayerDeath()
    {
        base.CheckForPlayerDeath();
        //enemy.enemyAttackBaseInstance.DoCheckForPlayerDeathLogic();
    }
    public override void AnimationTriggerEvent(EnemyHandler.AnimationTriggerType _animationTriggerType){}
}
