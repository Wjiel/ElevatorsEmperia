using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class House : MonoBehaviour, IInteractable
{
    [SerializeField] private HouseManager _manager;
    [SerializeField] private MoneyManager _moneyManager;

    [SerializeField] private GameObject displayCurrentCoins;
    public TextMeshProUGUI[] displayCurrentCountCoins;


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

        displayCurrentCoins.transform.DOScale(0, 0.2f)
           .SetEase(Ease.OutBounce)
           .OnComplete(() => displayCurrentCoins.SetActive(false));

        for (int i = 0; i < displayCurrentCountCoins.Length; i++)
            displayCurrentCountCoins[i].text = "0";
    }

    public void IsOwning()
    {
        StartCoroutine(AddCurrencyCoroutine());
    }

    private IEnumerator AddCurrencyCoroutine()
    {
        while (true)
        {
            currentCoins += liftCointReturn;

            displayCurrentCoins.SetActive(true);

            displayCurrentCoins.transform.localScale = Vector3.zero;
            displayCurrentCoins.transform.DOScale(1, 0.2f)
                .SetEase(Ease.InBounce);

            for (int i = 0; i < displayCurrentCountCoins.Length; i++)
                displayCurrentCountCoins[i].text = currentCoins.ToString();

            yield return new WaitForSeconds(interval);
        }
    }
}
