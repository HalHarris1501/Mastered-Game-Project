using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private Text moneyText;
    [SerializeField] private GameObject weaponSelectorUI;
    [SerializeField] private GameObject potionSelectorUI;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShowWeaponSwapper();
        ShowPotionUI();
    }

    public void UpdateHealth(int maxHealth, int currentHealth)
    {
        healthBar.maxValue = maxHealth;

        healthBar.value = currentHealth;

        healthText.text = currentHealth + "/" + maxHealth;
    }

    public void UpdateMoney(float money)
    {
        moneyText.text = "" + money.ToString("F2");
    }

    private void ShowWeaponSwapper()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !Player.Instance.currentWeapon.isAttacking)
        {
            weaponSelectorUI.SetActive(true);            
        }
        else
        {
            weaponSelectorUI.SetActive(false);
        }
        
    }

    private void ShowPotionUI()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            potionSelectorUI.SetActive(true);
        }
        else
        {
            potionSelectorUI.SetActive(false);
        }

    }
}
