using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponButton : MonoBehaviour
{
    [SerializeField] private Image weaponImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetIcon(WeaponType weaponType)
    {
        weaponImage.sprite = WeaponsLocker.Instance.GetWeaponIcon(weaponType);
    }
}
