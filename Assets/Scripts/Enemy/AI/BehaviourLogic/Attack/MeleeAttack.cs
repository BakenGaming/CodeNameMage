using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Behaviours/Enemy Attack Behaviours", fileName ="MeleeAttack")]
public class MeleeAttack : EnemyAttackSOBase
{
    public override void Initialize(GameObject _gameObject, EnemyHandler _enemy, Transform _playerTransform, EnemyStatsSO _stats)
    {
        gameObject = _gameObject;
        transform = gameObject.transform;
        enemy = _enemy;
        playerTransform = _playerTransform;
        stats = _stats;
    }

    public override void DoEnterStateLogic()
    {
        Debug.Log("Entered Attack");
    }
    public override void DoExitStateLogic()
    {
        Debug.Log("Exited Attack");
        ResetValues();
    }
    public override void DoFrameUpdateLogic(){}
    public override void DoPhysicsUpdateLogic(){}
    public override void DoCollisionHandlerLogic(){}
    public override void DoAnimationTriggerEventLogic(EnemyHandler.AnimationTriggerType _animationTriggerType){}
    public override void ResetValues(){}
}
