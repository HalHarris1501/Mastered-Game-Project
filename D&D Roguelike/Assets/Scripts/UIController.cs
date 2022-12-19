using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateHealth(int maxHealth, int currentHealth)
    {
        healthBar.maxValue = maxHealth;

        healthBar.value = currentHealth;

        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void UpdateMoney(float money)
    {
        moneyText.text = "" + money;
    }
}
