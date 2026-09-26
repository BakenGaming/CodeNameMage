using UnityEngine;

public class EnemyHandler : MonoBehaviour, IDamageable, IEnemyMoveable, ITriggerHandler
{
    [Header("Test Parameters")]
    public bool canMove;
    public bool canAttack;
    [Header("References")]
    public EnemyStatsSO stats;
    private DamageFlash _dmgFlash;
    public float maxHealth { get; set; }
    public float currenHealth { get; set; }
    public Rigidbody2D enemyRB { get; set; }
    public bool isFacingRight { get; set; } = true;
    public bool isAttacking { get; set; }
    public bool isAggroed { get; set; }
    #region State Machine Variables
    public EnemyStateMachine stateMachine { get; set; }
    public EnemyIdleState idleState  { get; set; }
    public EnemyChaseState chaseState  { get; set; }
    public EnemyAttackState attackState { get; set; }
    [SerializeField] private EnemyIdleSOBase enemyIdleBase;
    [SerializeField] private EnemeyChaseSOBase enemyChaseBase;
    [SerializeField] private EnemyAttackSOBase enemyAttackBase;
    public EnemyIdleSOBase enemyIdleBaseInstance { get; set; }
    public EnemeyChaseSOBase enemyChaseBaseInstance { get; set; }
    public EnemyAttackSOBase enemyAttackBaseInstance { get; set; }
    
    #endregion
    void Awake()
    {
        enemyIdleBaseInstance = Instantiate(enemyIdleBase);
        enemyChaseBaseInstance = Instantiate(enemyChaseBase);
        enemyAttackBaseInstance = Instantiate(enemyAttackBase);

        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _dmgFlash = GetComponent<DamageFlash>();
        maxHealth = stats.HP;
        currenHealth = maxHealth;
        enemyRB = GetComponent<Rigidbody2D>();

        enemyIdleBaseInstance.Initialize(gameObject, this, GameManager.i.playerGO.transform);
        enemyChaseBaseInstance.Initialize(gameObject, this, GameManager.i.playerGO.transform, stats);
        //enemyAttackBaseInstance.Initialize(gameObject, this, GameManager.i.GetPlayerTransform());

        stateMachine.Initialize(idleState);
        
        //GetComponent<EnemyAI>().Initialize();
    }
    public void TakeDamage(int _d, bool _isCrit)
    {
        _dmgFlash.CallDamageFlash();
        DamagePopup.Create(transform.position, _d, _isCrit);
        currenHealth -= _d;

        if(currenHealth <= 0) Die();
    }
    public void Die()
    {
        //Enqueue enemy
    }
    #region Movement Functions
    public void MoveEnemy(Vector2 velocity)
    {
        if(!canMove) return;
        enemyRB.linearVelocity = velocity;
        CheckForLeftOrRightFacing(velocity);
    }

    public void CheckForLeftOrRightFacing(Vector2 velocity)
    {
        if(isFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else if(!isFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }
    #endregion
    #region Trigger Handlers
    public void SetAggroStatus(bool _isAggroed)
    {
        isAggroed = _isAggroed;
    }
    public void SetAttacking(bool _isAttacking)
    {
        isAttacking = _isAttacking;
    }
    #endregion
    void Update()
    {
        stateMachine.currentEnemyState.FrameUpdate();
    }
    void FixedUpdate()
    {
        stateMachine.currentEnemyState.PhysicsUpdate();
        stateMachine.currentEnemyState.CollisionHandler();
        stateMachine.currentEnemyState.CheckForPlayerDeath();
    }
    #region Animation Triggers
    private void AnimationTriggerEvent(AnimationTriggerType _animationTriggerType)
    {
        stateMachine.currentEnemyState.AnimationTriggerEvent(_animationTriggerType);
    }
    public enum AnimationTriggerType
    {
        EnemyDamaged, PlayFootStepSounds
    }
    #endregion
}
