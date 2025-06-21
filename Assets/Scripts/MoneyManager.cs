using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI display;

    private int currentMoney = 0;

    private void Awake()
    {
        LoadCurrency();
    }

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void ModifyCoins(int amount)
    {
        currentMoney += amount;
        UpdateCurrencyUI();
        SaveCurrency();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("PlayerMoney", currentMoney);
        PlayerPrefs.Save();
    }

    private void LoadCurrency()
    {
        currentMoney = PlayerPrefs.GetInt("PlayerMoney", 0);
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        display.text = currentMoney.ToString();
    }
}


