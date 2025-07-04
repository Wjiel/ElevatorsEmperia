using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    [SerializeField] private HouseManager _manager;
    [SerializeField] private MoneyManager _moneyManager;

    [SerializeField] private GameObject displayCurrentCoins;

    [SerializeField] private TextMeshProUGUI countReturnDisplay;
    [SerializeField] private GameObject countReturnOblaco;

    public string buildingName = "��. �������� 12";
    public ElevatorData[] elevators;

    private void Start()
    {
        foreach (var elevator in elevators)
        {
            if (elevator.liftIsOwned)
            {
                StartCoroutine(AddCurrencyCoroutine(elevator));
            }
        }
    }
    public void startAddMoney()
    {
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

        _manager.toInteractHouse(elevators, this, buildingName);

        startAddMoney();
    }



    private IEnumerator AddCurrencyCoroutine(ElevatorData elevator)
    {
        while (true)
        {
            elevator.currentCoins += elevator.cointReturn;



            //if (!displayCurrentCoins.activeInHierarchy)
            //{
            //    displayCurrentCoins.SetActive(true);

            //    displayCurrentCoins.transform.localScale = Vector3.zero;
            //    displayCurrentCoins.transform.DOScale(1, 0.2f)
            //        .SetEase(Ease.InBounce);
            //}
            DisplayCurrentCoint();


            yield return new WaitForSeconds(elevator.interval);
        }
    }
    public void CollectAllCoins()
    {
        for (int i = 0; i < elevators.Length; i++)
        {
            _moneyManager.ModifyCoins(elevators[i].currentCoins);
            elevators[i].currentCoins = 0;
        }



        DisplayCurrentCoint();
    }

    public void DisplayCurrentCoint()
    {
        int allCoins = 0;


        for (int i = 0; i < elevators.Length; i++)
        {
            allCoins += elevators[i].currentCoins;
        }

        if (allCoins != 0)
            countReturnOblaco.SetActive(true);
        else
            countReturnOblaco.SetActive(false);

        if (allCoins >= 1000)
            allCoins = 999;

        countReturnDisplay.text = allCoins.ToString();
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

    public ElevatorLevelData[] levels;

    [HideInInspector] public int price;
    public int level;
    [HideInInspector] public int cointReturn;
    public bool liftIsOwned;

    public string liftName = "OTIS";
    [HideInInspector] public int WearResistance = 15;
    [HideInInspector] public int Reliability = 5;
    [HideInInspector] public int Convenience = 60;


    public float interval = 15f;
}


[Serializable]
public class ElevatorLevelData
{
    public int level;
    public int price;
    public int cointReturn;
    public int WearResistance = 15;
    public int Reliability = 5;
    public int Convenience = 60;
}