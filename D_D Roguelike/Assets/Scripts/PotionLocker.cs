using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionLocker : MonoBehaviour
{
    //Singleton pattern
    #region Singleton
    private static PotionLocker _instance;
    public static PotionLocker Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PotionLocker>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("PotionLocker");
                _instance = go.AddComponent<PotionLocker>();
            }
            return _instance;
        }
    }
    #endregion

    [SerializeField] private PotionStruct[] _potions = new PotionStruct[0];

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

    public Sprite GetPotionIcon(PotionEnum potionType)
    {
        for (int i = 0; i < _potions.Length; i++)
        {
            if (_potions[i].PotionType == potionType)
            {
                return _potions[i].Potion.GetComponent<SpriteRenderer>().sprite;
            }
        }
        Debug.LogError("Weapon not in locker");
        return null;
    }

    public GameObject GetPotionObject(PotionEnum potionType)
    {
        for (int i = 0; i < _potions.Length; i++)
        {
            if (_potions[i].PotionType == potionType)
            {
                return _potions[i].Potion;
            }
        }
        Debug.LogError("Potion not in locker");
        return null;
    }

}

