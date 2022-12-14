using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private int health = 3;
    [SerializeField] private bool unkillable = false;

    [SerializeField] private UIController uiManager = null;

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

    public void TakeDamage(int damageAmount, string damageType)
    {
        health -= damageAmount;

        if(this.gameObject.CompareTag("Player"))
        {
            uiManager.UpdateHealth(maxHealth, health);
        }

        if (health <= 0 && !unkillable)
        {
            Destroy(gameObject);
        }

        DamageIndicator indicator = ObjectPooler.Instance.SpawnFromPool("Damage Indicator", transform.position, Quaternion.identity).GetComponent<DamageIndicator>();
        indicator.SetDamageText(damageAmount);        
    }
}
