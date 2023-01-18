using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField] private bool isFriendly;
    [SerializeField] private bool isAmmo;
    private bool isCollectable = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration;
    [SerializeField]  int damage;
    [SerializeField] private DamageType damageType;
    private Vector2 targetPosition;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //point projectile to the direction it's facing
        Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        transform.up = direction;
        //moveSpeed += FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>().velocity;
        isCollectable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (!isAmmo)
        {
            if (duration > 0)
            {
                transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
                duration -= Time.fixedDeltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (duration > 0)
            {
                transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
                duration -= Time.fixedDeltaTime;
            }
            else
            {
                moveSpeed = 0;
                isCollectable = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAmmo)
        {
            if (isFriendly)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                    gameObject.SetActive(false);
                }

            }
            else
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                    gameObject.SetActive(false);
                }
            }

            if (collision.gameObject.CompareTag("Obstacle"))
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (!isCollectable)
            {
                if (isFriendly)
                {
                    if (collision.gameObject.CompareTag("Enemy"))
                    {
                        collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                        moveSpeed = 0;
                        isCollectable = true;
                    }

                }
                else
                {
                    if (collision.gameObject.CompareTag("Player"))
                    {
                        collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                        moveSpeed = 0;
                        isCollectable = true;
                    }
                }

                if (collision.gameObject.CompareTag("Obstacle"))
                {
                    moveSpeed = 0;
                    isCollectable = true;
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    collision.gameObject.GetComponentInChildren<Weapon>().IncreaseAmmo(1);
                    gameObject.SetActive(false);
                }
            }          
        }
    }

    public void SetVariables(bool friendly, float speed, float range, int damageToDo, DamageType damageTyping, float durationLength)
    {
        isFriendly = friendly;
        moveSpeed = speed;
        duration = range;
        damage = damageToDo;
        damageType = damageTyping;
        duration = durationLength;
    }
}
