using UnityEngine;

public class EnemyPatrol : EnemyState
{
   public EnemyPatrol(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    } 

    private Vector2 direction;
    private float decisionTimer;

  
   
    public override void EnterState()
    {   
        direction = Vector2.right * enemy.facingDir;
        decisionTimer = Random.Range(enemy.minDectime, enemy.maxDectime);
        enemy.anim.SetBool("Move", true);
        Debug.Log("I am in patrol");
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("Move", false);
        Debug.Log("I am leaving patrol");
    }

    public override void FrameUpdate()
    {   
         if(enemy.IsAggroed)
        {
            stateMachine.ChangeState(enemy.enemyChase);
           
        }

        decisionTimer -= Time.deltaTime;

        if(decisionTimer <= 0f)
        {
            decisionTimer = Random.Range(enemy.minDectime, enemy.maxDectime);
            enemy.MoveEnemy(Vector2.zero);

            if(enemy.RandomChance(45f))
            {
                stateMachine.ChangeState(enemy.enemyIdle);
            }

            else
            { 
             if(enemy.IsTherGround())
                {
                enemy.MoveEnemy(direction * enemy.enemySpeed); 
                }
            }
        }

        if(enemy.IsThereWall() || !enemy.IsTherGround())
            {   
                
                enemy.MoveEnemy(Vector2.zero);
                enemy.FlipSprite();
                stateMachine.ChangeState(enemy.enemyIdle);
                return;
                
            }  

         if(enemy.IsTherGround())
        {   
            enemy.MoveEnemy(direction * enemy.enemySpeed); 
        } 

        
        base.FrameUpdate(); 
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    

}
