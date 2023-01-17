using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionOfHealing : MonoBehaviour, IPotion
{
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
        int healing = 0;

        for(int i = 0; i < 2; i++)
        {
            healing += Random.Range(1, 4);
        }
        healing += 4;

        Player.Instance.GetComponentInChildren<HealthSystem>().TakeDamage(healing, DamageType.Healing);
    }
}
