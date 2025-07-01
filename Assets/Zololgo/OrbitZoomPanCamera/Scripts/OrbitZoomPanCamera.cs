using UnityEngine;

public class OrbitZoomPanCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float orbitSpeed = 1.0f;
    public float zoomSpeed = 1.0f;
    public float originDistance = 1.0f;
    public Transform originReference;

    private Camera cameraComponent;
    private Vector3 lastMousePosition;
    private Vector2 deltaMouseMovement;
    private Vector3 originPosition;

    private float currentOrbitSpeed;
    public bool IsMoving { get; private set; }

    private float yaw = 0f;
    private float pitch = 0f;
    private const float minPitch = -80f; 
    private const float maxPitch = 80f;  

    void Awake()
    {
        cameraComponent = Camera.main;
        lastMousePosition = Input.mousePosition;
        currentOrbitSpeed = orbitSpeed;
        CalculateOrigin();

        UpdateAnglesFromCurrentPosition();
    }

    void Update()
    {
        CalculateOrigin();
        HandleMouseInput();
        HandleTouchInput();

        lastMousePosition = Input.mousePosition;
    }

    void HandleMouseInput()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        deltaMouseMovement = currentMousePosition - lastMousePosition;

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            deltaMouseMovement = Vector2.zero;
        }

        if (Input.GetMouseButton(0))
        {
            // Обновляем углы
            yaw += deltaMouseMovement.x * currentOrbitSpeed * Time.deltaTime;
            pitch += deltaMouseMovement.y * currentOrbitSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch); 

            float radPitch = pitch * Mathf.Deg2Rad;
            float radYaw = yaw * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(
                Mathf.Sin(radYaw) * Mathf.Cos(radPitch),
                Mathf.Sin(radPitch),
                Mathf.Cos(radYaw) * Mathf.Cos(radPitch)
            );

            Vector3 newPosition = originPosition - direction * originDistance;

            cameraComponent.transform.position = newPosition;
            cameraComponent.transform.LookAt(originPosition);
        }

        bool isMouseMoving = (Input.GetMouseButton(0) || Input.GetMouseButton(1)) && deltaMouseMovement.magnitude > 0.1f;
        IsMoving = isMouseMoving;
    }

    void HandleTouchInput()
    {
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                float prevDistance = (touch0PrevPos - touch1PrevPos).magnitude;
                float currentDistance = (touch0.position - touch1.position).magnitude;

                float zoomModifier = (currentDistance - prevDistance) * zoomSpeed * Time.deltaTime;
                cameraComponent.fieldOfView = Mathf.Clamp(cameraComponent.fieldOfView - zoomModifier, 20f, 100f);

                currentOrbitSpeed = 0.5f;
                IsMoving = true;
            }
            else
            {
                currentOrbitSpeed = orbitSpeed;
                IsMoving = false;
            }
        }
        else
        {
            currentOrbitSpeed = orbitSpeed;
            IsMoving = false;
        }
    }

    void CalculateOrigin()
    {
        originPosition = originReference.position;

        originDistance = Vector3.Distance(cameraComponent.transform.position, originPosition);
    }

    void UpdateAnglesFromCurrentPosition()
    {
        Vector3 direction = originPosition - cameraComponent.transform.position;
        direction.Normalize();

        yaw = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        pitch = Mathf.Asin(direction.y) * Mathf.Rad2Deg;
    }
}