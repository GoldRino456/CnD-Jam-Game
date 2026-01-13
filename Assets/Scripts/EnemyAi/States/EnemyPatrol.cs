using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyPatrol : EnemyState
{
   public EnemyPatrol(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    } 

    private Vector2 direction;

    public override void EnterState()
    {   
        direction = Vector2.right;
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {   
         if(enemy.IsAggroed)
        {
            stateMachine.ChangeState(enemy.enemyChase);
            Debug.Log("Player Spotted");
        }
        
        enemy.MoveEnemy(direction * enemy.enemySpeed);
        if(enemy.IsThereWall())
        {
            direction = -direction;
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
