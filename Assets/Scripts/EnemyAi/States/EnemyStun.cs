using UnityEngine;

public class EnemyStun : EnemyState
{
    public EnemyStun(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        
    }

    private float stateTimer = 2f;

    public void SetStunTime(float time)
    {
        stateTimer = time;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.MoveEnemy(Vector2.zero);
        enemy.anim.SetTrigger("Idle");
    }

    public override void ExitState()
    {
        base.ExitState(); 
    }

    public override void FrameUpdate()
    { 
        base.FrameUpdate(); 
        stateTimer -= Time.deltaTime;

        if(stateTimer < 0)
        {   
            stateMachine.ChangeState(enemy.enemyPatrol);
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }
}
