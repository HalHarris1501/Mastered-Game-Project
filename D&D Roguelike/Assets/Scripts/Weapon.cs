using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;

    [SerializeField] private bool isMelee;
    [SerializeField] private int damageDice, versatileBonus;
    [SerializeField] private string damageType;
    [SerializeField] private float range;
    [SerializeField] private float weaponMoveSpeed;
    [SerializeField] private string[] properties = new string[0];
    [SerializeField] private bool isThrown;
    [SerializeField] private int ammunition;

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
        int damage = Random.Range(1, damageDice + 1);

        if (isThrown == true)
        {
            if (ammunition > 0)
            {
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);
                for(int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<HealthSystem>().TakeDamage(damage, damageType);
                }
                animator.SetTrigger("Attack");
                isAttacking = true;
            }
        }
        else
        {
            Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRadius, whatIsEnemy);
            for (int i = 0; i < enemiesToDamage.Length; i++)
            {
                enemiesToDamage[i].GetComponent<HealthSystem>().TakeDamage(damage, damageType);
            }
            animator.SetTrigger("Attack");
            isAttacking = true;
        }

    }

    public void RangedAttack()
    {
        if (ammunition > 0 || ammunition == -1)
        {
            GameObject editedProjectile = projectile;

            int damage = Random.Range(1, damageDice + 1);
            editedProjectile.GetComponent<Projectile>().SetVariables(true, weaponMoveSpeed, range, damage, damageType);

            if(ammunition > 0)
            {
                ammunition--;
            }

            Instantiate(editedProjectile, this.transform.position, Quaternion.identity);
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRadius);
    }
}
