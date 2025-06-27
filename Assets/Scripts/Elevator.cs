using System.Collections;
using TMPro;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using static UnityEngine.Analytics.IAnalytic;

public class Elevator : MonoBehaviour
{
    public TextMeshProUGUI nameDisplay;
    public TextMeshProUGUI levelDisplay;
    public TextMeshProUGUI priceDisplay;
    public TextMeshProUGUI priceLabelDisplay;

    private ElevatorData elevator;
    private HouseManager houseManager;
    private MoneyManager moneyManager;

    private void OnEnable()
    {
        UpdateButtonState();
    }
    public void Setup(ElevatorData data, MoneyManager money, HouseManager _houseManager)
    {
        elevator = data;
        moneyManager = money;
        houseManager = _houseManager;

        nameDisplay.text = data.liftName;
        levelDisplay.text = $"{data.level}";
    }
    private void UpdatePriceDisplay(int newAmount)
    {
        priceDisplay.text = newAmount.ToString();
    }
    private void OnDisable()
    {
            elevator.CurrentCoinsChanged -= UpdatePriceDisplay;
    }
    public void CollectCoins()
    {
        if (elevator.liftIsOwned)
        {
            moneyManager.ModifyCoins(elevator.currentCoins);

            elevator.currentCoins = 0;

            priceDisplay.text = "0";
        }
        else
        {
            houseManager.OpenLift(elevator);
        }
    }
    public void OpenLiftBt() => houseManager.OpenLift(elevator);

    void UpdateButtonState()
    {
        priceLabelDisplay.text = elevator.liftIsOwned ? "Забрать" : "Купить";
        priceDisplay.text = elevator.liftIsOwned ? elevator.currentCoins.ToString() : elevator.price.ToString();

        elevator.CurrentCoinsChanged += UpdatePriceDisplay;
    }
}
