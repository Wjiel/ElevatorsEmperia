using TMPro;
using UnityEngine;

public class HouseManager : MonoBehaviour
{
    [SerializeField] private MoneyManager moneyManager;

    [SerializeField] private GameObject panel;

    [SerializeField] private GameObject panelInfo;

    [SerializeField] private TextMeshProUGUI liftPriceText;

    [SerializeField] private TextMeshProUGUI liftText;

    [SerializeField] private TextMeshProUGUI liftNameText;

    [Header("Lift info")]
    [SerializeField] private TextMeshProUGUI liftNameText1;
    [SerializeField] private TextMeshProUGUI liftPriceText1;
    [SerializeField] private TextMeshProUGUI liftWearResistanceText;
    [SerializeField] private TextMeshProUGUI liftReliabilityText;
    [SerializeField] private TextMeshProUGUI liftConvenienceText;
    [SerializeField] private TextMeshProUGUI liftCollectText;
    [SerializeField] private TextMeshProUGUI liftPriceTextInfo;

    [SerializeField] private GameObject Bts;
    [SerializeField] private GameObject BtBuy;

    private House currentHouse;

    public void toInteractHouse(House house)
    {
        currentHouse = house;

        DisplayHouseInfo();
        panel.SetActive(true);
    }


    public void DisplayHouseInfo()
    {
        liftPriceText.text = currentHouse.liftIsOwned ? currentHouse.currentCoins.ToString() : currentHouse.liftPrice.ToString();
        liftPriceTextInfo.text = currentHouse.liftPrice.ToString();
        liftText.text = currentHouse.liftIsOwned ? "Собрать" : "Купить";
        liftNameText.text = currentHouse.liftName;
        liftNameText1.text = currentHouse.liftName;
        liftPriceText1.text = currentHouse.liftCointReturn.ToString();
        liftWearResistanceText.text = currentHouse.WearResistance.ToString();
        liftConvenienceText.text = currentHouse.Convenience.ToString();
        liftReliabilityText.text = currentHouse.Reliability.ToString();

        currentHouse.panelTextCurrCoints = liftCollectText;

        Bts.SetActive(currentHouse.liftIsOwned ? false : true);
        BtBuy.SetActive(currentHouse.liftIsOwned ? true : false);
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

