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
    [SerializeField] List<DamageStruct> damage;
    [SerializeField] private WeaponType weaponType;
    private Vector2 mousePosition;
    private float spread = 0.3f;
    private bool isCritical;

    // Start is called before the first frame update
    public void OnObjectSpawn()
    {
        GetTargetDirection();
        isCollectable = false;
    }

    private void GetTargetDirection()
    {
        //get mouse position
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y).normalized;

        Vector2 pdir = new Vector2();
        if (!isCritical)
        {
            //calculate spread for projectile
            pdir = Vector2.Perpendicular(direction);
        }
        else
        {
            pdir = Vector2.zero;
        }

        Vector2 newDirection = direction + (pdir * Random.Range(-spread, spread));

        //point projectile to the direction it's facing
        transform.up = newDirection;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        if (duration > 0)
        {
            transform.position += transform.up * moveSpeed * Time.fixedDeltaTime;
            duration -= Time.fixedDeltaTime;
        }
        else
        {
            DeactivateProjectile();
        }
    }

    private void DeactivateProjectile()
    {
        if (!isAmmo)
        {
            gameObject.SetActive(false);
        }
        else
        {
            moveSpeed = 0;
            isCollectable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ManageCollision(collision);
    }

    private void ManageCollision(Collider2D collision)
    {
        if (!isCollectable)
        {
            DetermineCollisionEffect(collision);
        }
        else
        {
            PickupProjectile(collision);
        }
    }

    private void DetermineCollisionEffect(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && isFriendly)
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, isCritical);
            DeactivateProjectile();
        }
        if (collision.gameObject.CompareTag("Player") && !isFriendly)
        {
            collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, isCritical);
            DeactivateProjectile();
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            DeactivateProjectile();
        }
    }

    private void PickupProjectile(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WeaponManager.Instance.AlterWeaponCount(weaponType, 1, true);
            gameObject.SetActive(false);
        }
    }

    public void SetVariables(ProjectileDataPack data, bool isCritical, List<DamageStruct> damage, bool isFriendly)
    {
        this.isFriendly = isFriendly;
        isAmmo = data.isAmmo;
        moveSpeed = data.moveSpeed;
        duration = data.duration;
        this.damage = damage;
        weaponType = data.weaponType;
        spread = data.spread;
        this.isCritical = isCritical;
    }
}
