using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TextMeshProUGUI upgradeLiftCost;

    public Image[] objectsCountReturn;

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

        currentLift.level = currentLift.levels[currentLift.level].level;

        if (currentLift.levels[currentLift.level].cointReturn != 0)
            currentLift.cointReturn = currentLift.levels[currentLift.level].cointReturn;

        currentLift.WearResistance = currentLift.levels[currentLift.level].WearResistance;
        currentLift.Reliability = currentLift.levels[currentLift.level].Reliability;
        currentLift.Convenience = currentLift.levels[currentLift.level].Convenience;


        liftProfitabilityText.text = currentLift.cointReturn.ToString() + "/мин";
        liftWearResistanceText.text = currentLift.WearResistance.ToString() + "%";
        liftConvenienceText.text = currentLift.Convenience.ToString() + "%";
        liftReliabilityText.text = currentLift.Reliability.ToString() + "%";

        liftPrice.text = currentLift.price.ToString();

        Bts.SetActive(currentLift.liftIsOwned ? true : false);
        BtBuy.SetActive(currentLift.liftIsOwned ? false : true);

        display(_nameHouse);

        collectCountReturn.text = currentLift.currentCoins.ToString();

        if (currentLift.level < currentLift.levels.Length - 1)
            upgradeLiftCost.text = currentLift.levels[currentLift.level + 1].price.ToString();


        for (int i = 0; i < objectsCountReturn.Length; i++)
        {
            if (i > currentLift.level)
                return;

            if (currentLift.levels[i].cointReturn != 0)
            {
                objectsCountReturn[i].color = Color.green;
            }
            else
            {
                objectsCountReturn[i].color = Color.white;
            }
        }
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
            moneyManager.DiscreCoins(currentLift.price);
            currentLift.liftIsOwned = true;
            DisplayHouseInfo();
            currentHouse.startAddMoney();
        }
    }

    public void UpgradeLevel()
    {
        if (currentLift.level < currentLift.levels.Length - 1)
            if (PlayerPrefs.GetInt("PlayerMoney", 0) >= currentLift.levels[currentLift.level + 1].price)
            {
                moneyManager.DiscreCoins(currentLift.levels[currentLift.level + 1].price);

                if (currentLift.levels[currentLift.level + 1].cointReturn != 0)
                    currentLift.cointReturn = currentLift.levels[currentLift.level + 1].cointReturn;
                currentLift.WearResistance = currentLift.levels[currentLift.level + 1].WearResistance;
                currentLift.Reliability = currentLift.levels[currentLift.level + 1].Reliability;
                currentLift.Convenience = currentLift.levels[currentLift.level + 1].Convenience;

                currentLift.level = currentLift.levels[currentLift.level + 1].level;
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

