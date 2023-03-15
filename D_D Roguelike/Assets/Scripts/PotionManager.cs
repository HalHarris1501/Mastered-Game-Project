using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour, ISubject<PotionEnum>
{
    private List<IObserver<PotionEnum>> _observers = new List<IObserver<PotionEnum>>();
    private Dictionary<PotionEnum, GameObject> _potions = new Dictionary<PotionEnum, GameObject>();
    private Dictionary<PotionEnum, int> _potionsCount = new Dictionary<PotionEnum, int>();

    //Singleton pattern
    #region Singleton
    private static PotionManager _instance;
    public static PotionManager Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PotionManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("PotionManager");
                _instance = go.AddComponent<PotionManager>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance == null) //if there's no instance of the potion manager, make this the potion manager, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //implement ISubject Interface
    public void RegisterObserver(IObserver<PotionEnum> o)
    {
        if (_observers.Count > 0)
        {
            if (_observers.Contains(o)) return; //check that o doesn't already exist in the list to avoid duplicates
        }
        _observers.Add(o);
    }

    public void RemoveObserver(IObserver<PotionEnum> o)
    {
        _observers.Remove(o);
    }

    public void NotifyObservers(PotionEnum potionType, ISubject<PotionEnum>.NotificationType notificationType)
    {
        //notify all observers that a new weapon has been added
        foreach (var observer in _observers)
        {
            if (notificationType == ISubject<PotionEnum>.NotificationType.Added)
            {
                observer.NewItemAdded(potionType);
            }
            else if (notificationType == ISubject<PotionEnum>.NotificationType.Removed)
            {
                observer.ItemRemoved(potionType);
            }
            else if (notificationType == ISubject<PotionEnum>.NotificationType.Changed)
            {
                observer.ItemCountAltered(potionType, _potionsCount[potionType]);
            }
        }
    }

    //called when the player picks up a potion so the potion manager can notify the observers
    public void AddPotionToInventory(PotionEnum potionType, GameObject potion)
    {
        if (_potions.ContainsKey(potionType))
        {
            _potionsCount[potionType]++;
        }
        else
        {
            _potionsCount.Add(potionType, 1);
            _potions.Add(potionType, potion);
            NotifyObservers(potionType, ISubject<PotionEnum>.NotificationType.Added);
        }
    }

    public void RemovePotionFromInventory(PotionEnum potionType)
    {
        if (!_potions.ContainsKey(potionType)) return;
        _potionsCount.Remove(potionType);
        _potions.Remove(potionType);
        NotifyObservers(potionType, ISubject<PotionEnum>.NotificationType.Removed);
    }

    public void AlterPotionCount(PotionEnum potionType, int numToAlterBy, bool increasingCount)
    {
        if (!_potionsCount.ContainsKey(potionType)) return;
        if (!increasingCount)
        {
            if (_potionsCount[potionType] > 1)
            {
                _potionsCount[potionType] -= numToAlterBy;
            }
            else if (_potionsCount[potionType] == 1)
            {
                RemovePotionFromInventory(potionType);
            }
        }
        else if (increasingCount)
        {
            _potionsCount[potionType] += numToAlterBy;
        }

        NotifyObservers(potionType, ISubject<PotionEnum>.NotificationType.Changed);
    }

    public GameObject GetPotion(PotionEnum potionType)
    {
        if (!_potions.ContainsKey(potionType)) //check that player still has potion in inventory
        {
            Debug.LogError(potionType + " not present in inventory");
            return null;
        }

        return _potions[potionType];
    }

    public bool CheckPotionInInventory(PotionEnum potionType)
    {
        if (_potions.ContainsKey(potionType))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CheckWeaponCount(PotionEnum potionType)
    {
        if (!_potionsCount.ContainsKey(potionType))
        {
            Debug.LogError(potionType + " not found in dictionary");
            return 0;
        }
        else
        {
            return _potionsCount[potionType];
        }
    }

    public void UsePotion(PotionEnum potionType)
    {
        if(_potions.ContainsKey(potionType))
        {
            if(_potionsCount[potionType] <= 0)
            {
                Debug.LogError(potionType + " not found in dictionary");
            }
            else
            {
                _potions[potionType].GetComponent<IPotion>().PotionEffect();
                AlterPotionCount(potionType, 1, false);
            }
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
