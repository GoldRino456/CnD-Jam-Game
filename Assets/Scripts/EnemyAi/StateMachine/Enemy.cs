using System;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovebale, ITriggerCheckable
{   

    #region Movement 

    public Vector2 currentVelocity;
    private float acceleration = 12f;
    public float enemySpeed = 5.2f;
    #endregion
    
    #region  Collision check
        
        [SerializeField] Transform wallCheckPostion;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] LayerMask IsThatWall; 
        
        [SerializeField] Transform groundCheckPosition;
        [SerializeField] float radius;
        [SerializeField] LayerMask IsThatGround;

    #endregion

    #region Flip Sprite
           
           public int facingDir {get; set;} = 1;
           public bool isFacingRight { get ; set ; } = true;
         
    #endregion

    #region Misc
    [SerializeField] public float minDectime;
    [SerializeField] public float maxDectime;
    #endregion

    [Header("States")]

    StateMachine stateMachine;
    public EnemyPatrol enemyPatrol;
    public EnemyChase enemyChase;
    public EnemyIdleState enemyIdle;

    public Rigidbody2D enemyRb { get ; set ; }
    public bool IsAggroed { get ; set ; }

    


    void Awake()
    {
        stateMachine = new StateMachine();
        enemyPatrol = new EnemyPatrol(this, stateMachine);
        enemyChase = new EnemyChase(this, stateMachine);
        enemyIdle = new EnemyIdleState(this, stateMachine);
    }
    void Start()
    {
        enemyRb = GetComponent<Rigidbody2D>();

        stateMachine.InitializeState(enemyPatrol);
    }

    
    void Update()
    {
        stateMachine.currentState.FrameUpdate();
    }

    void FixedUpdate()
    {
        stateMachine.currentState.PhysicsUpdate();
    }
    public void CheckIsFacingRight(Vector2 velocity)
    {
        if(velocity.x < 0 && isFacingRight)
        FlipSprite();

        if(velocity.x > 0 && !isFacingRight)
        FlipSprite();
    }

    public void MoveEnemy(Vector2 velocity)
    {  
        currentVelocity.x = Mathf.Lerp(currentVelocity.x, velocity.x, acceleration * Time.deltaTime);
        enemyRb.linearVelocity = velocity;
        CheckIsFacingRight(velocity);
    }

    public bool IsThereWall() => Physics2D.Raycast(wallCheckPostion.position, Vector2.right * facingDir, wallCheckDistance * facingDir, IsThatWall);
    public bool IsTherGround() => Physics2D.CircleCast(groundCheckPosition.position, radius, Vector2.down, 0f, IsThatGround);

    void OnDrawGizmos()
    {   Gizmos.color = Color.red;
        Vector3 rayDir = Vector3.right * facingDir * wallCheckDistance;
        Gizmos.DrawLine(wallCheckPostion.position, wallCheckPostion.position + rayDir);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPosition.position, radius);
    }

    public void FlipSprite()
    {
        facingDir = -facingDir;
        isFacingRight = !isFacingRight;
        transform.Rotate(0,180,0);
    }

    public void SetAgroStatus(bool isAggroed)
    {
        IsAggroed = isAggroed;
    }

    public bool RandomChance(float percent)
    {
        return UnityEngine.Random.value * 100 < percent;
    }
}
