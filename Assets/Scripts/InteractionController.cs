using UnityEngine;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionRange = 100f;

    private float intr = 0;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.touchCount == 1)
        {
            Vector2 screenPosition = Input.mousePosition;

            intr += Time.deltaTime;


            if (Input.GetMouseButtonUp(0) && Input.touchCount == 1)
            {
                if (intr < 0.2f)
                {
                    PerformInteraction(screenPosition);
                }
                intr = 0;
            }
        }
    }

    private void PerformInteraction(Vector2 screenPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactionLayer))
        {
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.OnInteract();
            }
        }
    }
}