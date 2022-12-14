using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject
{
    [SerializeField] private float value;

    [SerializeField] private MoneyManager moneyManager;

    [SerializeField] private float moveForce = 1f;

    [SerializeField] private float timer;
    private float moveTimer;

    public void OnObjectSpawn()
    {
        moveTimer = timer;
        moneyManager = FindObjectOfType<MoneyManager>();

        float xForce = Random.Range(-moveForce, moveForce);
        float yForce = Random.Range(-moveForce, moveForce);

        Vector2 force = new Vector2(xForce, yForce);

        GetComponent<Rigidbody2D>().velocity = force;
    }

    private void FixedUpdate()
    {
        //if (moveTimer > 0)
        //{
        //    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x * moveTimer, GetComponent<Rigidbody2D>().velocity.y * moveTimer);

        //    moveTimer -= 0.1f;
        //}
        //else
        //{
        //    GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            moneyManager.AddMoney(value);
            this.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            moveTimer = 0f;
        }
    }
}
