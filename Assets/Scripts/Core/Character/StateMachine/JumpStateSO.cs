using UnityEngine;

[CreateAssetMenu(menuName = "State/JumpStateSO")]
public class JumpStateSO : StateSO
{
    public override void Enter()
    {
        Debug.Log("Entering Jump State");
    }

    public override void StateUpdate()
    {
        Debug.Log("Updating Jump State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting Jump State");
    }


}
