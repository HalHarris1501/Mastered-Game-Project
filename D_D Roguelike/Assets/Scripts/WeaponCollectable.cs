using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollectable : MonoBehaviour
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(Input.GetKey(KeyCode.F))
        {
            WeaponManager.Instance.AddWeaponToInventory(weaponType, WeaponsLocker.Instance.GetWeaponObject(weaponType));
            Destroy(gameObject);
        }
    }
}
