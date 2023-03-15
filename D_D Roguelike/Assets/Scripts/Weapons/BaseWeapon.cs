using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWeapon : ScriptableObject
{
    public new string name;
    public WeaponDataPack weaponData;
    public bool isCrit;
    public LayerMask enemyLayer;
    public Vector2 damageBox;

    public virtual void Attack(GameObject weaponObject, int buttonPressed, bool isCritical) { }
}
