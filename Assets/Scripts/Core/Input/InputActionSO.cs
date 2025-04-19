using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Input System/Input Action", fileName = "New Input Action")]
public class InputActionSO : ScriptableObject
{
    // One Single Input Action Reference
    [SerializeField] InputActionReference inputActionReference;

    // ¡°When someone asks for InputActionReference, => just hand them back inputActionReference.¡±
    public InputActionReference InputActionReference => inputActionReference;

    // ¡°To do Enable(), => run inputActionReference.action.Enable().¡±
    public void Enable() => inputActionReference.action.Enable();
    public void Disable() => inputActionReference.action.Disable();

}
