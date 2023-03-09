using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon 
{
    ProjectileDataPack projectile { get; }
    GameObject gameObject { get;  }
    public bool isAttacking { get; }
    public void Attack(int attackType, bool isCritical);
}
