using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponValues", menuName = "Weapons/WeaponType/Dagger")]
public class Dagger : BaseWeapon
{
    [SerializeField] private ProjectileDataPack daggerProjectile;
    private GameObject weaponObject;
    

    public override void Attack(GameObject weaponObject, int buttonPressed, bool isCritical)
    {
        if (WeaponManager.Instance.CheckWeaponCount(weaponData.weaponType) <= 0) return;
        
        this.weaponObject = weaponObject;
        isCrit = isCritical;
        if (buttonPressed == 0)
        {
            MeleeAttack();
        }
        else if (buttonPressed == 1)
        {
            RangedAttack();
        }
    }

    private void RangedAttack()
    {        
        SpawnProjectile();        
    }

    private void SpawnProjectile()
    {
        GameObject editedProjectile = weaponObject.GetComponentInParent<WeaponParent>().rangedObjectPooler.SpawnProjectile(weaponObject.transform.position, Quaternion.identity, isCrit, weaponData.baseDamage);

        WeaponManager.Instance.AlterWeaponCount(weaponData.weaponType, 1, false);
    }

    private void MeleeAttack()
    {
        Collider2D enemy = Physics2D.OverlapBox(weaponObject.transform.position, damageBox, weaponObject.transform.rotation.eulerAngles.z, enemyLayer);

        if(enemy != null)
        {
            enemy.GetComponent<HealthSystem>().TakeDamage(weaponData.baseDamage, isCrit);
        }
    } 
}
