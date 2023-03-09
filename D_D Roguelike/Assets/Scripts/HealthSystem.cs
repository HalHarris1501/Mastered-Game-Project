using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private bool unkillable = false;
    [SerializeField] private int hitDie, numOfHitDie;

    [SerializeField] private UIController uiManager = null;
    [SerializeField] private List<DamageType> immunities;
    [SerializeField] private List<DamageType> resistances;
    [SerializeField] private List<DamageType> vulnerabilities;
    [SerializeField] private bool compoundIndicators;

    // Start is called before the first frame update
    void Start()
    {
        if (hitDie > 0 && numOfHitDie > 0)
        {
            maxHealth = hitDie;
            health = maxHealth;
        }

        if(this.gameObject.CompareTag("Player"))
        {
            uiManager = FindObjectOfType<UIController>();
            uiManager.UpdateHealth(maxHealth, health);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(List<DamageStruct> damages, bool isCrit)
    {
        CalculateDamage(damages, isCrit);

        ManageDamage();             
    }

    private void ManageDamage()
    {
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        if (GetComponent<Player>())
        {
            uiManager.UpdateHealth(maxHealth, health);
            CheckPlayerDead();
        }

        if (health <= 0 && !unkillable)
        {
            this.GetComponent<Enemy>().Death();
        }
    }

    private void CheckPlayerDead()
    {
        if (health <= 0 && !unkillable)
        {
            this.gameObject.SetActive(false);
        }
    }
    private void CalculateDamage(List<DamageStruct> damages, bool isCrit)
    {
        foreach (DamageStruct damageStruct in damages)
        {
            DamageIndicator indicator = ObjectPooler.Instance.SpawnFromPool("Damage Indicator", transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
            int damageAmount = DiceRoller.RollMultiple(damageStruct.damageDie, damageStruct.numOfDice);
            if(isCrit)
            {
                damageAmount += DiceRoller.RollMultiple(damageStruct.damageDie, damageStruct.numOfDice);
            }
            damageAmount += damageStruct.damageModifier;
            DamageType damageType = damageStruct.damageType;
            damageAmount = CheckResistances(damageAmount, damageType, indicator, isCrit);

            health -= damageAmount;
        }
    }

    private int CheckResistances(int damageAmount, DamageType damageType, DamageIndicator indicator, bool isCrit)
    {
        if (immunities.Contains(damageType))
        {
            damageAmount = 0;
            indicator.SetDamageText(damageAmount, Color.black);
        }
        else if (resistances.Contains(damageType))
        {
            damageAmount = Mathf.FloorToInt(damageAmount / 2);
            indicator.SetDamageText(damageAmount, Color.grey);
        }
        else if (vulnerabilities.Contains(damageType))
        {
            damageAmount = (damageAmount * 2);
            indicator.SetDamageText(damageAmount, Color.yellow);
        }
        else if (damageType == DamageType.Healing)
        {
            damageAmount = -damageAmount;
            indicator.SetDamageText(-damageAmount, Color.green);
        }
        else
        {
            indicator.SetDamageText(damageAmount, Color.red);
        }

        if (isCrit)
        {
            indicator.SetDamageText(damageAmount, Color.yellow);
        }
        return damageAmount;
    }
}
