using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, ISubject
{
    private Dictionary<WeaponType, GameObject> _weapons;
    private List<IObserver> _observers;
    //Singleton pattern
    #region Singleton
    private static WeaponManager _instance;
    public static WeaponManager Instance
    { 
        get //making sure that a weapon manager always exists
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<WeaponManager>();
            }
            if(_instance == null)
            {
                GameObject go = new GameObject("WeaponManager");
                _instance = go.AddComponent<WeaponManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if(_instance == null) //if there's no instance of the weapon manager, make this the weapons manager, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //implement ISubject Interface
    public void RegisterObserver(IObserver o)
    {
        Debug.Log(_observers.Count);
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver o)
    {
        _observers.Remove(o);
    }

    public void NotifyObservers(WeaponType weaponType)
    {
        //notify all observers that a new weapon has been added
        foreach(var observer in _observers)
        {
            observer.NewWeaponAdded(weaponType);
        }
    }

    //called when the player picks up a weapon so the weapon manager can notify the observers
    public void AddWeaponToInventory(WeaponType weaponType, GameObject weaponObject)
    {
        if (_weapons.ContainsKey(weaponType)) return; //check for weapon to prevent duplicates
        _weapons.Add(weaponType, weaponObject);
        NotifyObservers(weaponType);
    }

    public GameObject GetWeapon(WeaponType weaponType)
    {
        if(!_weapons.ContainsKey(weaponType)) //check that player still has weapon in inventory
        {
            Debug.LogError(weaponType + " not present in inventory");
            return null;
        }

        return _weapons[weaponType];        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
