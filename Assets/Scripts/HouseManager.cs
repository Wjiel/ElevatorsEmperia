using TMPro;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject Controller;

    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject panelInfo;
    [Header("House info")]

    [SerializeField] private TextMeshProUGUI[] liftPriceText;

    [SerializeField] private TextMeshProUGUI liftText;

    [SerializeField] private TextMeshProUGUI[] liftNameText;

    [Header("Lift info")]
    [SerializeField] private TextMeshProUGUI liftProfitabilityText;
    [SerializeField] private TextMeshProUGUI liftWearResistanceText;
    [SerializeField] private TextMeshProUGUI liftReliabilityText;
    [SerializeField] private TextMeshProUGUI liftConvenienceText;

    [SerializeField] private GameObject Bts;
    [SerializeField] private GameObject BtBuy;

    private House currentHouse;

    public void toInteractHouse(House house)
    {
        currentHouse = house;

        DisplayHouseInfo();
        panel.SetActive(true);
        Controller.SetActive(false);
    }


    public void DisplayHouseInfo()
    {
        for (int i = 0; i < liftNameText.Length; i++)
            liftNameText[i].text = currentHouse.liftName;

        liftText.text = currentHouse.liftIsOwned ? "Собрать" : "Купить";

        for (int i = 0; i < liftNameText.Length; i++)
            liftPriceText[i].text = currentHouse.liftIsOwned ? currentHouse.currentCoins.ToString() : currentHouse.liftPrice.ToString();

        currentHouse.displayCurrentCountCoins = liftPriceText;

        // Lift info

        liftProfitabilityText.text = currentHouse.liftCointReturn.ToString();
        liftWearResistanceText.text = currentHouse.WearResistance.ToString();
        liftConvenienceText.text = currentHouse.Convenience.ToString();
        liftReliabilityText.text = currentHouse.Reliability.ToString();

        Bts.SetActive(currentHouse.liftIsOwned ? true : false);
        BtBuy.SetActive(currentHouse.liftIsOwned ? false : true);
    }

    public void OpenLift()
    {
        if (currentHouse.liftIsOwned)
        {
            currentHouse.CollectCoins();
            return;
        }

        panelInfo.SetActive(true);
    }
    public void BuyHouse(int index)
    {
        if (currentHouse.liftIsOwned)
        {
            currentHouse.CollectCoins();
            return;
        }

        if (PlayerPrefs.GetInt("PlayerMoney", 0) >= currentHouse.liftPrice)
        {
            moneyManager.ModifyCoins(-currentHouse.liftPrice);
            currentHouse.liftIsOwned = true;
            DisplayHouseInfo();
            currentHouse.IsOwning();
        }

    }
}

