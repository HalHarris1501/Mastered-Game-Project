using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private bool unkillable = false;

    [SerializeField] private UIController uiManager = null;
    [SerializeField] private List<DamageType> immunities;
    [SerializeField] private List<DamageType> resistances;
    [SerializeField] private List<DamageType> vulnerabilities;

    // Start is called before the first frame update
    void Start()
    { 
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
        if (immunities.Contains(damageType))
        {
            damageAmount = 0;
            indicator.SetDamageText(damageAmount, Color.black);
        }
        else if (resistances.Contains(damageType))
        {
            damageAmount = Mathf.FloorToInt(damageAmount / 2);
            if(damageAmount == 0)
            {
                damageAmount = 1;
            }
            indicator.SetDamageText(damageAmount, Color.grey);
        }
        else if(vulnerabilities.Contains(damageType))
        {
            damageAmount = (damageAmount * 2);
            indicator.SetDamageText(damageAmount, Color.yellow);
        }
        else if(damageType == DamageType.Healing)
        {
            damageAmount = -damageAmount;
            indicator.SetDamageText(-damageAmount, Color.green);
        }
        else
        {
            indicator.SetDamageText(damageAmount, Color.red);
        }

        health -= damageAmount;

        if(health > maxHealth)
        {
            health = maxHealth;
        }

        if (this.gameObject.CompareTag("Player"))
        {
            uiManager.UpdateHealth(maxHealth, health);
        }

        if (health <= 0 && !unkillable)
        {
            Destroy(gameObject);
        }            
    }
}
