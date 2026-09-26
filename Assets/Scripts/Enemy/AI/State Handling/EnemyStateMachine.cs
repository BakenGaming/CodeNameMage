using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentEnemyState {get; set;}

    public void Initialize(EnemyState _state)
    {
        currentEnemyState = _state;
        currentEnemyState.EnterState();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentEnemyState.ExitState();
        currentEnemyState = _newState;
        currentEnemyState.EnterState();
    }
}
