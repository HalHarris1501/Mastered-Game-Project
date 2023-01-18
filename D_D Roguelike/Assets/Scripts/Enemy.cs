using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private float damageTimer = 1f;
    [SerializeField] private float challengeRating;

    public void OnObjectSpawn()
    {
        targetObject = FindObjectOfType<Player>().gameObject;
    }

    private void FixedUpdate()
    {
        if (targetObject != null)
        {
            var step = moveSpeed * Time.fixedDeltaTime;
            var position = Vector2.MoveTowards(transform.position, targetObject.transform.position, step);
            transform.position = position;
        }

        if(damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (damageTimer <= 0f)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(1, DamageType.Piercing);
                damageTimer = 1f;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (damageTimer <= 0f)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(1, DamageType.Piercing);
                damageTimer = 1f;
            }
        }
    }

    public void Death()
    {
        GenerateCoins();

        gameObject.SetActive(false);
    }

    private void GenerateCoins()
    {
        int coinsToSpawn = Random.Range(0, (100 + Mathf.RoundToInt(100 * challengeRating)));

        for (int i = coinsToSpawn; i > 0; i--)
        {            
            if (i > 100)
            {
                i++;
                ObjectPooler.Instance.SpawnFromPool("Gold Piece", transform.position, Quaternion.identity);
                i -= 100;
            }
            else if(i > 10)
            {
                i++;
                ObjectPooler.Instance.SpawnFromPool("Silver Piece", transform.position, Quaternion.identity);
                i -= 10;
            }
            else
            {
                ObjectPooler.Instance.SpawnFromPool("Copper Piece", transform.position, Quaternion.identity);
            }
        }
        
        
    }
}
