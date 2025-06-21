using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    [SerializeField] private HouseManager _manager;
    [SerializeField] private MoneyManager _moneyManager;

    [SerializeField] private GameObject displayCurrentCoins;
    public TextMeshProUGUI panelTextCurrCoints;




    public int liftPrice;
    public int liftCointReturn;
    public bool liftIsOwned;

    public string liftName = "OTIS";
    public int WearResistance = 15;
    public int Reliability = 5;
    public int Convenience = 60;


    public float interval = 15f;
    public int currentCoins = 0;


    public void OnInteract()
    {
        transform.DOScale(0.9f, 0.1f).SetEase(Ease.InOutCubic).OnComplete(() => transform.DOScale(1f, 0.3f));

        _manager.toInteractHouse(this);
    }
    public void CollectCoins()
    {
        _moneyManager.ModifyCoins(currentCoins);

        currentCoins = 0;
        displayCurrentCoins.SetActive(false);

        panelTextCurrCoints.text = 0.ToString();
    }

    public void IsOwning()
    {
        StartCoroutine(AddCurrencyCoroutine());
    }

    IEnumerator AddCurrencyCoroutine()
    {
        while (true)
        {
            currentCoins += liftCointReturn;
            displayCurrentCoins.SetActive(true);

            panelTextCurrCoints.text = currentCoins.ToString();

            yield return new WaitForSeconds(interval);
        }
    }
}
