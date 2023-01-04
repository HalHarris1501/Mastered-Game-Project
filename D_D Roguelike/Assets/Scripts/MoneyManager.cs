using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private UIController uIController;
    [SerializeField] private static float money = 0.00f;

    // Start is called before the first frame update
    void Start()
    {
        uIController = FindObjectOfType<UIController>();
        uIController.UpdateMoney(money);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetMoney(float moneyToSet)
    {
        money = moneyToSet;
        uIController.UpdateMoney(money);
    }

    public void AddMoney(float moneyToAdd)
    {
        money += moneyToAdd;
        uIController.UpdateMoney(money);
    }
}
