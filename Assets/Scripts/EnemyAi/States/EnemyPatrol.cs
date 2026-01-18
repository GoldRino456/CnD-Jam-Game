using System.Collections;
using UnityEditor.Experimental.GraphView;
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
        direction = Vector2.right;
        decisionTimer = Random.Range(enemy.minDectime, enemy.maxDectime);
        enemy.anim.SetBool("Move", true);
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
         enemy.anim.SetBool("Move", false);
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
                enemy.MoveEnemy(direction * enemy.enemySpeed * enemy.facingDir); 
            }
        }

        if(enemy.IsThereWall())
            {
                stateMachine.ChangeState(enemy.enemyIdle);
                direction = -direction;  
            }  

         if(enemy.IsTherGround())
        {   
            enemy.MoveEnemy(direction * enemy.enemySpeed * enemy.facingDir); 
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
