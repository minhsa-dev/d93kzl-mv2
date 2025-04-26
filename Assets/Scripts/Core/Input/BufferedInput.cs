using UnityEngine;

public struct BufferedInput 
{

    // Defines the data structure for buffered input events.
    public enum ActionType
    {
        Move,
        Jump,
        Crouch,
        Sprint,
        Zoom,
        Look
    }

    public ActionType Action;
    public float eventTime; // Timestamp (Time.time) when the event occurred
}
