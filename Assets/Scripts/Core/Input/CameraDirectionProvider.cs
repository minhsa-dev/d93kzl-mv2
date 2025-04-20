using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

/// <summary>Gives camera‑space XZ axes in world coordinates.</summary>
/// 
public class CameraDirectionProvider : MonoBehaviour
{
    public static CameraDirectionProvider Instance { get; private set; }


    private Transform cameraTransform;
    Vector2 cachedInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }

        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }



    private void CachedInput(Vector2 value)
    {
        cachedInput = value;
    }





    // Update is called once per frame
    void LateUpdate()
    {
        if (cachedInput.sqrMagnitude < 0.01f)
        {

            return;
        }
        if (cachedInput.sqrMagnitude > 0.01f)
        {
            Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
            Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;

            Vector3 cameraRelative = (cameraForward * cachedInput.y + cameraRight * cachedInput.x).normalized;


        }


    }
}
