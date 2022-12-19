using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private bool isFriendly;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float duration;
    private Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //point projectile to the direction it's facing
        Vector2 direction = new Vector2(targetPosition.x - transform.position.x, targetPosition.y - transform.position.y);
        transform.up = direction;
        //moveSpeed += FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>().velocity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (duration > 0)
        {
            transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
            duration -= Time.fixedDeltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isFriendly)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(1);
                Destroy(gameObject);
            }
            
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
        
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(gameObject);
        }

    }
}
