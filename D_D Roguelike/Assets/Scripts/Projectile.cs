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
    [SerializeField] private WeaponType weaponType;
    private Vector2 mousePosition;
    private float offset = -90f;
    private float spread = 0.3f;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        GetTargetDirection();   
        //moveSpeed += FindObjectOfType<Player>().gameObject.GetComponent<Rigidbody2D>().velocity;
        isCollectable = false;
    }

    private void GetTargetDirection()
    {
        //get mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //calculate spread for projectile
        float angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector2 dir = transform.rotation * Vector2.up;
        Vector2 pdir = Vector2.Perpendicular(dir) * Random.Range(-spread, spread);

        //point projectile to the direction it's facing
        transform.up = dir + pdir;
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
                    WeaponManager.Instance.AlterWeaponCount(weaponType, 1, true);
                    gameObject.SetActive(false);
                }
            }          
        }
    }

    public void SetVariables(bool friendly, float speed, float range, int damageToDo, DamageType damageTyping, float durationLength, WeaponType thisWeaponType)
    {
        isFriendly = friendly;
        moveSpeed = speed;
        duration = range;
        damage = damageToDo;
        damageType = damageTyping;
        duration = durationLength;
        weaponType = thisWeaponType;
    }
}
