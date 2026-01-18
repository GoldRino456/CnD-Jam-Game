using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    }


    public override void EnterState()
    {
        base.EnterState();
         enemy.anim.SetBool("Attack", true);
        
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.anim.SetBool("Attack", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if(!enemy.IsAttacking)
        {
            stateMachine.ChangeState(enemy.enemyChase);
        }
        enemy.ProcessItemTimer();

        
        enemy.ProcessThrow();
        // if(enemy.isThrown == true)
        // {
        //    
        //     enemy.isThrown = false;
        // }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
    
 }
