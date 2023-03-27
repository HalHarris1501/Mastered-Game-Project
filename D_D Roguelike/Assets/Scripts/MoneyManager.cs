using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour, ISubject<float>
{
    [SerializeField] private TMP_Text playerMoneyText;
    [SerializeField] private static float money = 1000.00f;
    private List<IObserver<float>> _observers = new List<IObserver<float>>();
    bool firstUpdate = true;

    //Singleton pattern
    #region Singleton
    private static MoneyManager _instance;
    public static MoneyManager Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MoneyManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("MoneyManager");
                _instance = go.AddComponent<MoneyManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance == null) //if there's no instance of the weapon manager, make this the weapons manager, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(firstUpdate)
        {
            UpdateMoney();
            firstUpdate = false;
        }
    }

    //implement ISubject Interface
    public void RegisterObserver(IObserver<float> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<float> o)
    {
        _observers.Remove(o);
    }

    public void NotifyObservers(float money)
    {
        //notify all observers that money has been altered
        foreach (var observer in _observers)
        {
            observer.ItemAltered(money, Mathf.RoundToInt(money));
        }
    }

    public void SetMoney(float moneyToSet)
    {
        money = moneyToSet;
        UpdateMoney();
    }

    public float GetMoney()
    {
        return money;
    }

    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        UpdateMoney();
    }

    private void UpdateMoney()
    {
        playerMoneyText.text = money + "G";
        NotifyObservers(money);
    }

    public void NotifyObservers(float type, ISubject<float>.NotificationType notificationType)
    {
        throw new System.NotImplementedException();
    }
}
