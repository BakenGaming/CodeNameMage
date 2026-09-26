using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Behaviours/Enemy Chase Behaviours", fileName ="DirectToTargetBehaviour")]
public class DirectToTarget : EnemeyChaseSOBase
{
    private float movementSpeed;
    private float collisionRadius = .5f;
    public override void Initialize(GameObject _gameObject, EnemyHandler _enemy, Transform _playerTransform, EnemyStatsSO _stats)
    {
        gameObject = _gameObject;
        transform = gameObject.transform;
        enemy = _enemy;
        playerTransform = _playerTransform;
        stats = _stats;
        movementSpeed = stats.SPEED;
    }

    public override void DoEnterStateLogic()
    {
        Debug.Log("Entered Chase");
    }
    public override void DoExitStateLogic()
    {
        Debug.Log("Exited Chase");
        ResetValues();
    }
    public override void DoFrameUpdateLogic()
    {
        Vector2 moveDirection = (playerTransform.position - enemy.transform.position).normalized;
        enemy.MoveEnemy(moveDirection * movementSpeed);
    }
    public override void DoPhysicsUpdateLogic(){}
    public override void DoCollisionHandlerLogic()
    {
        Collider2D colliders = Physics2D.OverlapCircle(transform.position, collisionRadius, GameManager.i.staticVariables.GetPlayerLayer());
        if(colliders != null)
        {
            //enemy.CollideWithPlayer();
        }
    }
    public override void DoCheckForPlayerDeathLogic()
    {
        if(playerTransform == null)
        {
            enemy.stateMachine.ChangeState(enemy.idleState);
        }
    }
    public override void DoAnimationTriggerEventLogic(EnemyHandler.AnimationTriggerType _animationTriggerType){}
    public override void ResetValues(){}
}
