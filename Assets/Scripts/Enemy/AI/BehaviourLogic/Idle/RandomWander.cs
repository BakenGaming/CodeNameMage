using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Behaviours/Enemy Wander Behaviours", fileName ="RandomWanderBehaviour")]
public class RandomWander : EnemyIdleSOBase
{
    #region Idle Variables
    [SerializeField] private float randomMovementRange = 5f;
    [SerializeField] private float randomMovementSpeed = 1f;
    private Vector3 targetPosition, direction;
    #endregion
    public override void Initialize(GameObject _gameObject, EnemyHandler _enemy, Transform _playerTransform)
    {
        gameObject = _gameObject;
        transform = gameObject.transform;
        enemy = _enemy;
        playerTransform = _playerTransform;
    }

    public override void DoEnterStateLogic()
    {
        base.DoEnterStateLogic();
        
    }
    public override void DoExitStateLogic()
    {
        base.DoExitStateLogic();
    }
    public override void DoFrameUpdateLogic()
    {
        base.DoFrameUpdateLogic();

        direction = (targetPosition - enemy.transform.position).normalized;
        enemy.MoveEnemy(direction * randomMovementSpeed);

        if((enemy.transform.position - targetPosition).sqrMagnitude < .01f)
        {
            targetPosition = GetRandomPointInCircle();
        }
    }
    public override void DoPhysicsUpdateLogic()
    {
        base.DoPhysicsUpdateLogic();
    }
    public override void DoCollisionHandlerLogic()
    {
        base.DoCollisionHandlerLogic();
    }
    public override void DoAnimationTriggerEventLogic(EnemyHandler.AnimationTriggerType _animationTriggerType){}

    private Vector3 GetRandomPointInCircle()
    {
        return enemy.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * randomMovementRange;
    }
    public override void ResetValues(){}
}
