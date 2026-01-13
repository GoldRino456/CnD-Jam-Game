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
            enemy.MoveEnemy(moveDirection *_moveSpeed);
        }

        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
