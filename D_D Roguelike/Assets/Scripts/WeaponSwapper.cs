using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapper : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;

    // Start is called before the first frame update
    void Start()
    {
        SetCurrentWeapon(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentWeapon(int weaponIndex)
    {
        Player.Instance.SetWeapon(weapons[weaponIndex].GetComponent<Weapon>(), weapons[weaponIndex].GetComponent<SpriteRenderer>());        
    }
}
