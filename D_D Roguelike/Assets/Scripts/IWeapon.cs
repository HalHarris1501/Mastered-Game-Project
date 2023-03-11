using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    ProjectileDataPack projectile { get; }

    WeaponDataPack weaponData { get; }
    public bool isAttacking { get; }
    public bool isCrit { get; set; }
    public void Attack(int attackType, bool isCritical);
}
