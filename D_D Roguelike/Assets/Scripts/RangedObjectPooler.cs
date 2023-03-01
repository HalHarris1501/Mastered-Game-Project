using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RangedObjectPooler : MonoBehaviour
{
    [SerializeField] private ProjectilePool pool;
    [SerializeField] private bool isFriendly;
    Queue<GameObject> objectPool;
    [SerializeField] private ProjectileDataPack currentProjectile;

    [System.Serializable]
    public struct ProjectilePool
    {
        public GameObject prefab;
        public int size;
    }

    // Start is called before the first frame update
    void Start()
    {
        initialise();
    }

    private void initialise()
    {
        objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++) //adds all objects wanted to the queue, in an inactive state
        {
            GameObject obj = Instantiate(pool.prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }
    }

    public GameObject SpawnProjectile(Vector2 position, Quaternion rotation, bool isCritical, int damage)
    {
        GameObject objectToSpawn = objectPool.Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.localScale = currentProjectile.transform.localScale;
        objectToSpawn.GetComponent<SpriteRenderer>().sprite = currentProjectile.sprite;

        objectToSpawn.GetComponent<Projectile>().SetVariables(currentProjectile, isCritical, damage, isFriendly);

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;      

        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        objectPool.Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void SetDataPack(ProjectileDataPack dataPack)
    {
        currentProjectile = dataPack;
    }
}

