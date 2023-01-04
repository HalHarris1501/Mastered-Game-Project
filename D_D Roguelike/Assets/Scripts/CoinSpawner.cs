using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        ObjectPooler.Instance.SpawnFromPool("Gold Piece", transform.position, Quaternion.identity);
    }
}
