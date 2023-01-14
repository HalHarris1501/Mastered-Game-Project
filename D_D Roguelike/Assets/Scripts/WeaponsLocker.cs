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


        return null;
    }

}
