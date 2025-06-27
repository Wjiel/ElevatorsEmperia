using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    [SerializeField] private HouseManager _manager;
    [SerializeField] private MoneyManager _moneyManager;

    [SerializeField] private GameObject displayCurrentCoins;

    public string buildingName = "Дом";
    public ElevatorData[] elevators;
    public void startAddMoney() {
        StopAllCoroutines();

        foreach (var elevator in elevators)
        {
            if (elevator.liftIsOwned)
            {
                StartCoroutine(AddCurrencyCoroutine(elevator));
            }
        }
    }

    public void OnInteract()
    {
        transform.DOScale(0.9f, 0.1f).SetEase(Ease.InOutCubic).OnComplete(() => transform.DOScale(1f, 0.3f));

         _manager.toInteractHouse(elevators, this);

        startAddMoney();
    }


    private IEnumerator AddCurrencyCoroutine(ElevatorData elevator)
    {
        while (true)
        {
            elevator.currentCoins += elevator.cointReturn;

            //displayCurrentCoins.SetActive(true);

            //displayCurrentCoins.transform.localScale = Vector3.zero;
            //displayCurrentCoins.transform.DOScale(1, 0.2f)
            //    .SetEase(Ease.InBounce);

            yield return new WaitForSeconds(elevator.interval);
        }
    }   


}

[Serializable]
public class ElevatorData 
{
    public event Action<int> CurrentCoinsChanged; 

    private int _currentCoins;
    public int currentCoins
    {
        get => _currentCoins;
        set
        {
            if (_currentCoins != value)
            {
                _currentCoins = value;
                CurrentCoinsChanged?.Invoke(_currentCoins);
            }
        }
    }

    public int price;
    public int level;
    public int cointReturn;
    public bool liftIsOwned;

    public string liftName = "OTIS";
    public int WearResistance = 15;
    public int Reliability = 5;
    public int Convenience = 60;


    public float interval = 15f;
}