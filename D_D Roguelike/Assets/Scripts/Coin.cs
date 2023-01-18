using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IPooledObject
{
    [SerializeField] private float value;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private float moveForce = 3f;
    [SerializeField] private float duration;
    
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float moveTimer;
    private float percentageComplete;

    [SerializeField] private AnimationCurve curve;


    public void OnObjectSpawn()
    {
        startPosition = transform.position;
        moneyManager = FindObjectOfType<MoneyManager>();

        float xForce = Random.Range(-moveForce, moveForce);
        float yForce = Random.Range(-moveForce, moveForce);
        moveTimer = 0;

        endPosition = new Vector2(transform.position.x + xForce, transform.position.y + yForce);
    }

    void Update()
    {
        moveTimer += Time.deltaTime;
        percentageComplete = moveTimer / duration;

        transform.position = Vector2.Lerp(startPosition, endPosition, curve.Evaluate(percentageComplete));
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
            endPosition = transform.position;
        }
    }
}
