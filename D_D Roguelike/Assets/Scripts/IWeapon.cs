using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    ProjectileDataPack projectile { get; set; }
    WeaponDataPack weaponData { get; set; }
    public bool isAttacking { get; }
    public bool isCrit { get; set; }
    public void Attack(int attackType, bool isCritical);
}
