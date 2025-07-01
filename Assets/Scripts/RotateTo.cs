using UnityEngine;

public class RotateTo : MonoBehaviour
{
   
    private Camera targetCamera; 
    [SerializeField] private float rotationSpeed = 5f;

    private Transform thisTransform;
    private Transform cameraTransform;

    void Start()
    {
        thisTransform = transform;

        targetCamera = Camera.main;

        cameraTransform = targetCamera.transform;

    }

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 directionToCamera = cameraTransform.position - thisTransform.position;

        directionToCamera.y = 0f;

        if (directionToCamera.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera, Vector3.up);

            thisTransform.rotation = Quaternion.Slerp(thisTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
