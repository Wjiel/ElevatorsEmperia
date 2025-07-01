using UnityEngine;

public class CollectAllCoins : MonoBehaviour, IInteractable
{
    [SerializeField] private House house;
    public void OnInteract()
    {
        house.CollectAllCoins();
    }
}
