using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedObjectPooler : MonoBehaviour
{
    [SerializeField] private List<ProjectilePool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private ProjectileDataPack currentProjectile;

    [System.Serializable]
    public struct ProjectilePool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (ProjectilePool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //creates a queue full of objects

            for (int i = 0; i < pool.size; i++) //adds all objects wanted to the queue, in an inactive state
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(tag, objectPool); //adds the pool to the dictionary
        }
    }

    public GameObject SpawnProjectile(Vector2 position, Quaternion rotation)
    {
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void SetDataPack(ProjectileDataPack dataPack)
    {
        currentProjectile = dataPack;
    }
}

