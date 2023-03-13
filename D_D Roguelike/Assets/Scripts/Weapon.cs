using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [SerializeField] private ProjectileDataPack _projectile;
    [SerializeField] private WeaponDataPack _weaponData;
    [SerializeField] private bool offHandEmpty;
    [SerializeField] private WeaponType weaponType;

    [SerializeField] private bool isMelee;
    [SerializeField] private List<DamageStruct> damage;
    [SerializeField] private DamageStruct versatileDamage;
    [SerializeField] private float range;
    [SerializeField] private float weaponMoveSpeed;
    [SerializeField] private string[] properties = new string[0];
    [SerializeField] private bool isThrown;
    [SerializeField] private float duration;
    [SerializeField] private bool infiniteAmmo = false;
    [SerializeField] private int ammo;
    private bool canDealDamage;
    private DamageStruct baseDamage;
    public bool isCrit { get; set; }

    [SerializeField] private Collider2D weaponCollider;

    [SerializeField] private Animator animator;
    public bool isAttacking { get; private set; }
    public ProjectileDataPack projectile  { get { return _projectile; } set { ProjectileDataPack projectileDataPack = _projectile; } }
    public WeaponDataPack weaponData { get {return _weaponData; } set { WeaponDataPack weaponDataPack = _weaponData; } }

    public void ResetIsAttacking()
    {
        isAttacking = false;
        weaponCollider.enabled = false;
    }

    private void Start()
    {
        Activate();
    }

    public void Activate()
    { 
        weaponCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsThrown();
    } 

    private void CheckIsThrown()
    {
        if (isThrown)
        {
            CheckWeaponCount();
        }
    }

    private void CheckWeaponCount()
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

    private void SetIsAttacking()
    {
        if (WeaponManager.Instance.CheckWeaponCount(weaponType) > 0)
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }        
    }

    private void MeleeAttack()
    {
        canDealDamage = true;
        weaponCollider.enabled = true;

        CheckForVersatile();

        if (isThrown)
        {
            SetIsAttacking();
        }
        else
        {
            animator.SetTrigger("Attack");
            isAttacking = true;
        }
    }

    private void CheckForVersatile()
    {
        for (int i = 0; i < damage.Count; i++)
        {
            if (damage[i].damageType == versatileDamage.damageType && offHandEmpty)
            {
                damage[i] = versatileDamage;
            }
            else if(damage[i].damageType == versatileDamage.damageType)
            {
                damage[i] = baseDamage;
            }
        }
    }

    private void RangedAttack()
    {
        if (WeaponManager.Instance.CheckWeaponCount(weaponType) > 0 || ammo > 0 || infiniteAmmo)
        {
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        GameObject editedProjectile = GetComponentInParent<WeaponParent>().rangedObjectPooler.SpawnProjectile(this.transform.position, Quaternion.identity, isCrit, damage);

        if (!infiniteAmmo && isThrown)
        {
            WeaponManager.Instance.AlterWeaponCount(weaponType, 1, false);
        }
        else
        {
            ammo--;
        }
    }

    public void Attack(int buttonPressed, bool isCritical)
    {
        isCrit = isCritical;
        if(isMelee == true)
        {
            if(buttonPressed == 0)
            {
                MeleeAttack();
            }
            else if(buttonPressed == 1)
            {
                if (isThrown == true)
                {
                    RangedAttack();
                }                
            }
        }
        else
        {
            RangedAttack();
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

    public void AttemptDamage(Collider2D collision)
    {
        if(isAttacking && canDealDamage)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<HealthSystem>().TakeDamage(damage, isCrit);
                canDealDamage = false;
            }
        }
    }
}
