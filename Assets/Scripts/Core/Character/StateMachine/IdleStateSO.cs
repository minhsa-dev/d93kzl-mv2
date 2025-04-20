using UnityEngine;

[CreateAssetMenu(menuName = "State/IdleStateSO")]
public class IdleStateSO : StateSO
{
    public override void Enter()
    {
        Debug.Log("Entering Idle State");
    }

    public override void StateUpdate()
    {
        Debug.Log("Updating Idle State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State");
    }


}
