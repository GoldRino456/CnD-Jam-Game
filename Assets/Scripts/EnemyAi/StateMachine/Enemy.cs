using System;
using UnityEditor.Animations;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IMovebale, ITriggerCheckable, IAttackable
{   
    [Header("Movement")]
    #region Movement 

    public Vector2 currentVelocity;
    private float acceleration = 12f;
    public float enemySpeed = 5.2f;
    #endregion
    
    [Header("Collision Settings")]
    #region  Collision check
        
        [SerializeField] Transform wallCheckPostion;
        [SerializeField] private float wallCheckDistance;
        [SerializeField] LayerMask IsThatWall; 
        
        [SerializeField] Transform groundCheckPosition;
        [SerializeField] float radius;
        [SerializeField] LayerMask IsThatGround;
         [SerializeField] private float activeRange = 20f;

    #endregion

    
    #region Flip Sprite
           
           public int facingDir {get; set;} = 1;
           public bool isFacingRight { get ; set ; } = true;
         
    #endregion

    [Space]
    
    [Header("Misc Settings")]
    #region Misc
    [SerializeField] public float minDectime;
    [SerializeField] public float maxDectime;
    protected Vector2 _lastPlayerPosition;
    public HolyWater holyWater;
    #endregion

    [Space]
    
    [Header("Suspicion Settings")]
    #region Sus Sus 
    [SerializeField] private float playerDetectDistance = 10f;
    [SerializeField] private float playerDetectResolution = 15f;

    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask obstacleLayer;

    #endregion

    [Space]

    [Header("Attack Settings")]
    #region Attack
    [SerializeField] private float playerAttackDistance = 7f;
    [SerializeField] private float playerAttackResolution = 15f;
    [SerializeField] protected GameObject holyWaterPrefab;
    [SerializeField] private Transform throwSpawnPoint;
    [SerializeField] private float itemUseCooldown = 12f;
   
    public bool isThrown = false;
    public float itemUseTimer = 0f;

    public bool IsAggroed { get ; set ; }
    public bool IsAttacking { get; set; }
    #endregion

    [Space]

    [Header("States")]

    StateMachine stateMachine;
    public EnemyPatrol enemyPatrol;
    public EnemyChase enemyChase;
    public EnemyIdleState enemyIdle;
    public EnemyAttack enemyAttack;

    [Space]

    [Header("Animations")]
    
    public Animator anim;
    public Rigidbody2D enemyRb { get ; set ; }
    

    void Awake()
    {
        stateMachine = new StateMachine();
        enemyPatrol = new EnemyPatrol(this, stateMachine);
        enemyChase = new EnemyChase(this, stateMachine);
        enemyIdle = new EnemyIdleState(this, stateMachine);
        enemyAttack = new EnemyAttack(this, stateMachine);

        holyWater = FindAnyObjectByType<HolyWater>();
        anim = GetComponent<Animator>();
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
        {
        FlipSprite();
        
        }

        if(velocity.x > 0 && !isFacingRight)
        {
        FlipSprite();
        
        }
    }

    public void MoveEnemy(Vector2 velocity)
    {  
        currentVelocity.x = Mathf.Lerp(currentVelocity.x, velocity.x, acceleration * Time.deltaTime);
        enemyRb.linearVelocity = velocity;
        CheckIsFacingRight(velocity);
    }

    public bool IsThereWall() => Physics2D.Raycast(wallCheckPostion.position, Vector2.right * facingDir, wallCheckDistance * facingDir, IsThatWall);
    public bool IsTherGround() => Physics2D.CircleCast(groundCheckPosition.position, radius, Vector2.down, 0f, IsThatGround);
    public bool RaycastChaseSweep()
    {
        for (int i = 0; i < playerDetectResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerDetectResolution - 1));

            RaycastHit2D hit =
                Physics2D.Raycast(transform.position + Vector3.up * 1f, Quaternion.Euler(0, 0, angle) * Vector3.right * facingDir, playerDetectDistance, playerLayer | obstacleLayer | groundLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _lastPlayerPosition = hit.collider.transform.position;
                return true;
            }
        }

        return false;
    }

    public bool RaycastAttackSweep()
    {
        for (int i = 0; i < playerAttackResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerAttackResolution - 1));

            RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.up * 1f, Quaternion.Euler(0,0,angle) * Vector3.right * facingDir, playerAttackDistance, playerLayer | obstacleLayer | groundLayer);
            if(hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _lastPlayerPosition = hit.collider.transform.position;
                return true;
            }
        }

        return false;
    }

    void OnDrawGizmos()
    {   Gizmos.color = Color.red;
        Vector3 rayDir = Vector3.right * facingDir * wallCheckDistance;
        Gizmos.DrawLine(wallCheckPostion.position, wallCheckPostion.position + rayDir);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundCheckPosition.position, radius);

         Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, activeRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * facingDir * playerDetectDistance);

        for (int i = 0; i < playerDetectResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerDetectResolution - 1));
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right * facingDir;

            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f + dir * playerDetectDistance);
        }

        Gizmos.color = Color.coral;
          for (int i = 0; i < playerAttackResolution; i++)
        {
            float angle = Mathf.Lerp(-45f, 45f, (float)i / (playerAttackResolution - 1));
            Vector3 dir = Quaternion.Euler(0, 0, angle) * Vector3.right * facingDir;

            Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 1f + dir * playerAttackDistance);
        }
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

    public void SetAttackStatus(bool isAttacking)
    {
        IsAttacking = isAttacking;
    }


    #region Plague Doctor Attack
    public void ProcessItemTimer()
    {
        itemUseTimer -= Time.deltaTime;
        Debug.Log(itemUseTimer);

        if (itemUseTimer < 0f)
        {
            itemUseTimer = 0f;
            
        }

       


    }
    
    public void ProcessThrow()
    {
       if(itemUseTimer <=0 ) //&& !isThrown
        {   
            isThrown = true;
            var newHolyWater = Instantiate(holyWaterPrefab, throwSpawnPoint.position, Quaternion.identity);
            newHolyWater.GetComponent<HolyWater>().OnThrown(enemyRb.linearVelocity, !isFacingRight);
            itemUseTimer = itemUseCooldown;
                 
           
        }
    }
    #endregion
}
