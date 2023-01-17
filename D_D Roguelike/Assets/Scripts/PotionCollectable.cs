using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCollectable : MonoBehaviour, ICollectable<PotionEnum>
{
    [SerializeField] private PotionEnum potionType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public PotionEnum Pickup()
    {
        Destroy(gameObject);
        return potionType;
    }
}
