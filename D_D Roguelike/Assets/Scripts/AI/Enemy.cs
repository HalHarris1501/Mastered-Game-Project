using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private List<DamageStruct> damage;
    [SerializeField] private float damageTimer;
    [SerializeField] private float startDamageTimer = 1f;
    [SerializeField] private float challengeRating;
    [SerializeField] private Animator myAnimator;
    private float sleepTimer;

    public void OnObjectSpawn()
    {
        //targetObject = Player.Instance.gameObject;
        damageTimer = startDamageTimer;
        myAnimator = GetComponent<Animator>();
    }

    private void Awake()
    {
        //targetObject = Player.Instance.gameObject;
        damageTimer = startDamageTimer;
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        /*if (targetObject != null)
        {
            var step = moveSpeed * Time.fixedDeltaTime;
            var position = Vector2.MoveTowards(transform.position, targetObject.transform.position, step);
            transform.position = position;
        }*/             
    }

    private void Update()
    {
        if(sleepTimer > 0)
        {
            sleepTimer -= Time.deltaTime;
        }
        else
        {
            myAnimator.SetBool("isAsleep", true);
        }

        if (damageTimer > 0f)
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
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, false);
                damageTimer = startDamageTimer;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (damageTimer <= 0f)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, false);
                damageTimer = startDamageTimer;
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

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public void CanSeePlayer()
    {
        myAnimator.SetBool("canSeeEnemy", true);
    }

    public void AwakenEnemy()
    {
        Debug.Log("Wake me up");
        sleepTimer = 5f;
        myAnimator.SetBool("isAsleep", false);        
    }
}
