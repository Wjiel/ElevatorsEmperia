using DG.Tweening;
using TMPro;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject Controller;

    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject panelInfo;
    [SerializeField] private GameObject panelLifts;

    [Header("House info")]
    [SerializeField] private TextMeshProUGUI nameDisplay;
    [SerializeField] private TextMeshProUGUI countDisplay;

    private string _nameHouse;

    [Header("Lift info")]
    [SerializeField] private TextMeshProUGUI liftName;
    [SerializeField] private TextMeshProUGUI liftProfitabilityText;
    [SerializeField] private TextMeshProUGUI liftWearResistanceText;
    [SerializeField] private TextMeshProUGUI liftReliabilityText;
    [SerializeField] private TextMeshProUGUI liftConvenienceText;

    [SerializeField] private TextMeshProUGUI liftPrice;

    [SerializeField] private GameObject BtBuy;
    [SerializeField] private GameObject Bts;

    [SerializeField] private TextMeshProUGUI collectCountReturn;

    private House currentHouse;
    private ElevatorData[] currentLifts;
    private ElevatorData currentLift;

    public Elevator elevatorUIPrefab;
    public Transform elevatorListContainer;

    public void toInteractHouse(ElevatorData[] elevators, House house, string nameHouse)
    {
        if (currentLift != null)
            currentLift.CurrentCoinsChanged -= UpdatePriceDisplay;

        currentLifts = elevators;
        currentHouse = house;

        foreach (Transform child in elevatorListContainer)
            Destroy(child.gameObject);

        foreach (var elevator in elevators)
        {
            Elevator uiElement = Instantiate(elevatorUIPrefab, elevatorListContainer);
            uiElement.Setup(elevator, moneyManager, this);
        }

        _nameHouse = nameHouse;

        display(_nameHouse);

        panel.SetActive(true);
        panelLifts.SetActive(true);
        Controller.SetActive(false);
    }
    private void display(string nameHouse)
    {
        nameDisplay.text = nameHouse;

        int countBuyedElevators = 0;
        for (int i = 0; i < currentLifts.Length; i++)
        {
            if (currentLifts[i].liftIsOwned)
            {
                countBuyedElevators++;
            }
        }
        countDisplay.text = $"{countBuyedElevators}/{currentLifts.Length}";
    }

    public void toReturnCoins()
    {
        if (currentHouse != null)
            currentHouse.DisplayCurrentCoint();
    }

    public void DisplayHouseInfo()
    {
        liftName.text = currentLift.liftName;

        liftProfitabilityText.text = currentLift.cointReturn.ToString() + "/мин";
        liftWearResistanceText.text = currentLift.WearResistance.ToString() + "%";
        liftConvenienceText.text = currentLift.Convenience.ToString() + "%";
        liftReliabilityText.text = currentLift.Reliability.ToString() + "%";

        liftPrice.text = currentLift.price.ToString();

        Bts.SetActive(currentLift.liftIsOwned ? true : false);
        BtBuy.SetActive(currentLift.liftIsOwned ? false : true);

        display(_nameHouse);

        collectCountReturn.text = currentLift.currentCoins.ToString();
    }

    public void OpenLift(ElevatorData currLift)
    {
        if (currentLift != null)
            currentLift.CurrentCoinsChanged -= UpdatePriceDisplay;

        currentLift = currLift;

        panelInfo.SetActive(true);
        DisplayHouseInfo();

        panelLifts.SetActive(false);

        currentLift.CurrentCoinsChanged += UpdatePriceDisplay;
    }
    public void BuyHouse()
    {
        if (PlayerPrefs.GetInt("PlayerMoney", 0) >= currentLift.price)
        {
            moneyManager.ModifyCoins(-currentLift.price);
            currentLift.liftIsOwned = true;
            DisplayHouseInfo();
            currentHouse.startAddMoney();
        }
    }

    public void CollectCoins()
    {
        moneyManager.ModifyCoins(currentLift.currentCoins);

        currentLift.currentCoins = 0;
    }

    private void UpdatePriceDisplay(int newAmount)
    {
        collectCountReturn.text = newAmount.ToString();
    }

}

