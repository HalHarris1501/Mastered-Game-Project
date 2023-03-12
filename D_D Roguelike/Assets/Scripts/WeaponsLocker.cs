using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsLocker : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static WeaponsLocker _instance;
    public static WeaponsLocker Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeaponsLocker>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("WeaponsLocker");
                _instance = go.AddComponent<WeaponsLocker>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private WeaponStruct[] _weapons = new WeaponStruct[0];
    
    private void Awake()
    {
        if (_instance == null) //if there's no instance of the weapon locker, make this the weapons locker, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sprite GetWeaponIcon(WeaponType weaponType)
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i].Type == weaponType)
            {
                return _weapons[i].weaponSprite;
            }            
        }
        Debug.LogError("Weapon not in locker");
        return null;        
    }

    public IWeapon GetWeaponObject(WeaponType weaponType)
    {
        for(int i = 0; i < _weapons.Length; i++)
        {
            if(_weapons[i].Type == weaponType)
            {
                return _weapons[i].weapon;
            }
        }
        Debug.LogError("Weapon not in locker");
        return null;        
    }
}
