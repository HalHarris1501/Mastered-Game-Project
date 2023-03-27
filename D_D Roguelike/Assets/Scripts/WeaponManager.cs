using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour, ISubject<WeaponType>
{
    private Dictionary<WeaponType, BaseWeapon> _weapons = new Dictionary<WeaponType, BaseWeapon>();
    private Dictionary<WeaponType, int> _weaponCount = new Dictionary<WeaponType, int>();
    private List<IObserver<WeaponType>> _observers = new List<IObserver<WeaponType>>();

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

    public void NotifyObservers(WeaponType weaponType, ISubject<WeaponType>.NotificationType notificationType)
    {
        //notify all observers that a new weapon has been added
        foreach(var observer in _observers)
        {
            if (notificationType == ISubject<WeaponType>.NotificationType.Added)
            {
                observer.NewItemAdded(weaponType);
            }
            else if (notificationType == ISubject<WeaponType>.NotificationType.Removed)
            {
                observer.ItemRemoved(weaponType);
            }
            else if (notificationType == ISubject<WeaponType>.NotificationType.Changed)
            {
                observer.ItemAltered(weaponType, _weaponCount[weaponType]);
            }
        }
    }

    //called when the player picks up a weapon so the weapon manager can notify the observers
    public void AddWeaponToInventory(WeaponType weaponType, BaseWeapon weaponObject)
    {
        if (_weapons.ContainsKey(weaponType))
        {
            _weaponCount[weaponType]++;
        }
        else
        {
            _weaponCount.Add(weaponType, 1);
            _weapons.Add(weaponType, weaponObject);
            NotifyObservers(weaponType, ISubject<WeaponType>.NotificationType.Added);
        }
    }

    public void RemoveWeaponFromInventory(WeaponType weaponType)
    {
        if (!_weapons.ContainsKey(weaponType)) return;
        _weaponCount.Remove(weaponType);
        _weapons.Remove(weaponType);
        NotifyObservers(weaponType, ISubject<WeaponType>.NotificationType.Removed);       
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

        NotifyObservers(weaponType, ISubject<WeaponType>.NotificationType.Changed);
    }

    public BaseWeapon GetWeapon(WeaponType weaponType)
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
}
