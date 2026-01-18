using UnityEngine;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    }


    public override void EnterState()
    {
        base.EnterState();
        
    }

    public override void ExitState()
    {
        base.ExitState();
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
