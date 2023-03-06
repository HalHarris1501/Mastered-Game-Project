using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionOfHealing : MonoBehaviour, IPotion
{
    [SerializeField] private List<DamageStruct> potionEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PotionEffect()
    {
        Player.Instance.GetComponentInChildren<HealthSystem>().TakeDamage(potionEffect, false);
    }
}
