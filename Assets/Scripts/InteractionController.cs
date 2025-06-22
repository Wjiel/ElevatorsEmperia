using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private OrbitZoomPanCamera orbitCamera;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRange = 100f; 


    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {

        if (orbitCamera.IsMoving || IsPointerOverUI())
            return;



        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                PerformInteraction(touch.position);
            }
        }
    }

    bool IsPointerOverUI()
    {
        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
    }

    void PerformInteraction(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, interactionLayer))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract();
            }
        }
    }
}