using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, ISubject<WeaponType>
{
    private Dictionary<WeaponType, GameObject> _weapons = new Dictionary<WeaponType, GameObject>();
    private Dictionary<WeaponType, int> _weaponCount = new Dictionary<WeaponType, int>();
    private List<IObserver<WeaponType>> _observers = new List<IObserver<WeaponType>>();
    private bool addingWeapon;

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
    public void RegisterObserver(IObserver<WeaponType> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<WeaponType> o)
    {
        _observers.Remove(o);
    }

    public void NotifyObservers(WeaponType weaponType)
    {
        //notify all observers that a new weapon has been added
        foreach(var observer in _observers)
        {
            if (addingWeapon)
            {
                observer.NewItemAdded(weaponType);
            }
            else
            {
                observer.ItemRemoved(weaponType);
            }
        }
    }

    //called when the player picks up a weapon so the weapon manager can notify the observers
    public void AddWeaponToInventory(WeaponType weaponType, GameObject weaponObject)
    {
        if (_weapons.ContainsKey(weaponType))
        {
            _weaponCount[weaponType]++;
        }
        else
        {
            _weaponCount.Add(weaponType, 1);
            _weapons.Add(weaponType, weaponObject);
            addingWeapon = true;
            NotifyObservers(weaponType);
        }
    }

    public void RemoveWeaponFromInventory(WeaponType weaponType)
    {
        if (!_weapons.ContainsKey(weaponType)) return;
        _weaponCount.Remove(weaponType);
        _weapons.Remove(weaponType);
        addingWeapon = false;
        NotifyObservers(weaponType);       
    }

    public void AlterWeaponCount(WeaponType weaponType, int numToAlterBy, bool increasingCount)
    {
        if (!_weaponCount.ContainsKey(weaponType)) return;
        if (!increasingCount)
        {
            if (_weaponCount[weaponType] > 0)
            {
                _weaponCount[weaponType] -= numToAlterBy;
            }
            else if (_weaponCount[weaponType] <= 0)
            {
                Debug.LogError("No " + weaponType + "s left in inventory");
            }
        }
        else if (increasingCount)
        {
            _weaponCount[weaponType] += numToAlterBy;
        }         
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

    public bool CheckWeaponInInventory(WeaponType weaponType)
    {
        if(_weapons.ContainsKey(weaponType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CheckWeaponCount(WeaponType weaponType)
    {
        if (!_weaponCount.ContainsKey(weaponType))
        {
            Debug.LogError(weaponType + " not found in dictionary");
            return 0;
        }
        else
        {
            return _weaponCount[weaponType];
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
}
