using System.ComponentModel.Design;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyState 
{
    protected Enemy enemy;
    protected StateMachine stateMachine;

    public EnemyState(Enemy _enemy, StateMachine _stateMachine)
    {
       enemy = _enemy;
       stateMachine = _stateMachine;
    }

    public virtual void EnterState()
    {
        
    }

    public virtual void ExitState()
    {
        
    }

    public virtual void FrameUpdate()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void AnimationTrigger()
    {
        
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
