using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectable : MonoBehaviour, ICollectable<WeaponType>
{
    [SerializeField] private WeaponType weaponType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public WeaponType Pickup()
    {
        Destroy(gameObject);
        return weaponType;
    }
}
