using UnityEngine;

public class StateMachine 
{
   public EnemyState currentState {get; set;}

   public void InitializeState(EnemyState startingState)
    {
        currentState = startingState;
        currentState.EnterState();
    }

    public void ChangeState(EnemyState nextState)
    {
        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }
}
