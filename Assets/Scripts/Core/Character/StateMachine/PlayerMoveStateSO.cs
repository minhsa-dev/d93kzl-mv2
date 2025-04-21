using UnityEngine;

[CreateAssetMenu(menuName = "State/PlayerMoveStateSO")]
public class PlayerMoveStateSO : StateSO
{
    public override void Enter()
    {
        Debug.Log("Entering Move State");
    }

    public override void StateUpdate()
    {
        Debug.Log("Updating Move State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Move State");
    }


}
