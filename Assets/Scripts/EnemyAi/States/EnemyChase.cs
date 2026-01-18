using UnityEditor.ShaderGraph;
using UnityEngine;

public class EnemyChase : EnemyState
{   
    Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 5.2f;
      public EnemyChase(Enemy _enemy, StateMachine _stateMachine) : base(_enemy, _stateMachine)
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.anim.SetTrigger("Moving");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {   
        if (_playerTransform != null)
        {
            Vector2 moveDirection;
            moveDirection = (_playerTransform.position - enemy.transform.position).normalized;
            enemy.MoveEnemy(new Vector2(moveDirection.x * _moveSpeed, 0f));
        }

        if(!enemy.IsAggroed)
        {
            stateMachine.ChangeState(enemy.enemyPatrol);
            
        }

        if(enemy.IsAttacking)
        {
            stateMachine.ChangeState(enemy.enemyAttack);
            
        }

        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
