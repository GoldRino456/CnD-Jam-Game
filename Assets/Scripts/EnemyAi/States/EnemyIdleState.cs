using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    }

    private float stateTimer = 2f;
    bool hasChanged;

    public override void EnterState()
    {
        base.EnterState();
        stateTimer = 2f;
        hasChanged = false;
        enemy.MoveEnemy(Vector2.zero);
    }

    public override void ExitState()
    {
        base.ExitState(); 
        
    }

    public override void FrameUpdate()
    { 
        base.FrameUpdate(); 
        stateTimer -= Time.deltaTime;

        if( !hasChanged && stateTimer <=0 )
        {   
            hasChanged = true;
            stateMachine.ChangeState(enemy.enemyPatrol);
        }

        Debug.Log(stateTimer);
        
    }
}
