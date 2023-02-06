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

    public void TakeDamage(int damageAmount, DamageType damageType)
    {
        DamageIndicator indicator = ObjectPooler.Instance.SpawnFromPool("Damage Indicator", transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        health -= CalculateDamage(damageAmount, damageType, indicator);

        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if (this.gameObject.CompareTag("Player"))
        {
            uiManager.UpdateHealth(maxHealth, health);
            if (health <= 0 && !unkillable)
            {
                this.gameObject.SetActive(false);
            }
        }

        if (health <= 0 && !unkillable)
        {
            this.GetComponent<Enemy>().Death();
        }            
    }

    private int CalculateDamage(int damageAmount, DamageType damageType, DamageIndicator indicator)
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
        return damageAmount;
    }
}
