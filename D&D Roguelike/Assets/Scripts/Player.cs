using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D rigidBody;
    private Vector2 movement;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject shootPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(weapon, new Vector3(shootPoint.transform.position.x, shootPoint.transform.position.y, 0), Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
