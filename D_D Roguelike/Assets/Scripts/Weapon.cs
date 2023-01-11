using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private bool offHandEmpty;

    [SerializeField] private bool isMelee;
    [SerializeField] private int damageDice, versatileDice;
    [SerializeField] private string damageType;
    [SerializeField] private float range;
    [SerializeField] private float weaponMoveSpeed;
    [SerializeField] private string[] properties = new string[0];
    [SerializeField] private bool isThrown;
    [SerializeField] private int ammunition;
    [SerializeField] private float duration;
    private bool canDealDamage;

    [SerializeField] private Transform attackPos;
    [SerializeField] private float attackRadius;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private Animator animator;
    public bool isAttacking { get; private set; }

    public void ResetIsAttacking()
    {
        isAttacking = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMelee || isThrown)
        {
            if (ammunition == 0)
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }  

    public void MeleeAttack()
    {
        canDealDamage = true;
        int damage = calculateDamage();

        if (isThrown == true)
        {
            if (ammunition > 0)
            {
                //Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);
                //for(int i = 0; i < enemiesToDamage.Length; i++)
                //{
                //    enemiesToDamage[i].GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                //}
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else
        {
            //Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);
            //for (int i = 0; i < enemiesToDamage.Length; i++)
            //{
            //    enemiesToDamage[i].GetComponent<HealthSystem>().TakeDamage(damage, damageType);
            //}
            animator.SetTrigger("Attack");
            isAttacking = true;
        }

    }

    public void RangedAttack()
    {
        if (ammunition > 0 || ammunition == -1)
        {
            int damage = calculateDamage();
            
            GameObject editedProjectile = ObjectPooler.Instance.SpawnFromPool(projectile.name, this.transform.position, Quaternion.identity);                       
            editedProjectile.GetComponent<Projectile>().SetVariables(true, weaponMoveSpeed, range, damage, damageType, duration);

            if(ammunition > 0)
            {
                ammunition--;
            }
        }
    }

    public void Attack(int buttonPressed)
    {
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
        ammunition += increase;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    //}

    private int calculateDamage()
    {
        int damage = 0;
        if(offHandEmpty == true && versatileDice != 0)
        {
            damage = Random.Range(1, versatileDice + 1);
        }
        else
        {
            damage = Random.Range(1, damageDice + 1);
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
