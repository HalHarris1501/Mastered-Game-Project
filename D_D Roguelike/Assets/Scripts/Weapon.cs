using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ProjectileDataPack projectile;
    [SerializeField] private bool offHandEmpty;
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private bool isMelee;
    [SerializeField] private int damageDice, versatileDice;
    [SerializeField] private DamageType damageType;
    [SerializeField] private float range;
    [SerializeField] private float weaponMoveSpeed;
    [SerializeField] private string[] properties = new string[0];
    [SerializeField] private bool isThrown;
    [SerializeField] private float duration;
    [SerializeField] private bool infiniteAmmo = false;
    [SerializeField] private int ammo;
    private bool canDealDamage;

    [SerializeField] private Collider2D weaponCollider;

    [SerializeField] private Animator animator;
    public bool isAttacking { get; private set; }

    public void ResetIsAttacking()
    {
        isAttacking = false;
        weaponCollider.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            if (WeaponManager.Instance.CheckWeaponCount(weaponType) <= 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;             
            }
        }
    }  

    public void MeleeAttack(bool isCritical)
    {
        canDealDamage = true;
        weaponCollider.enabled = true;

        if (isThrown)
        {
            if (WeaponManager.Instance.CheckWeaponCount(weaponType) > 0)
            {                
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }

    }

    public void RangedAttack(bool isCritical)
    {
        if (WeaponManager.Instance.CheckWeaponCount(weaponType) > 0 || ammo > 0 || infiniteAmmo)
        {
            int damage = calculateDamage();
            
            GameObject editedProjectile = GetComponentInParent<WeaponParent>().rangedObjectPooler.SpawnProjectile(this.transform.position, Quaternion.identity, isCritical, damage);                       

            if(!infiniteAmmo && isThrown)
            {
                WeaponManager.Instance.AlterWeaponCount(weaponType, 1, false);
            }
            else
            {
                ammo--;
            }
        }
    }

    public void Attack(int buttonPressed, bool isCritical)
    {
        if(isMelee == true)
        {
            if(buttonPressed == 0)
            {
                MeleeAttack(isCritical);
            }
            else if(buttonPressed == 1)
            {
                if (isThrown == true)
                {
                    RangedAttack(isCritical);
                }                
            }
        }
        else
        {
            RangedAttack(isCritical);
        }
    }

    public void IncreaseAmmo(int increase)
    {
        if (!isThrown)
        {
            ammo += increase;
        }
        else
        {
            WeaponManager.Instance.AlterWeaponCount(weaponType, increase, true);
        }
    }

    private int calculateDamage()
    {
        int damage;
        if(offHandEmpty == true && versatileDice != 0)
        {
            damage = DiceRoller.DiceRoll(versatileDice);
        }
        else
        {
            damage = DiceRoller.DiceRoll(damageDice);
        }

        return damage;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(isAttacking && canDealDamage)
        {
            int damage = calculateDamage();
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                canDealDamage = false;
            }
        }
    }
}
