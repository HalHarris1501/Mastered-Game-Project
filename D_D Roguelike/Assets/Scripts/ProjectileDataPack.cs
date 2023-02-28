using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDataPack : ScriptableObject
{
    public Sprite sprite;
    public Transform transform;
    public bool isFriendly;
    public bool isAmmo;
    public bool isCollectable = false;
    public float moveSpeed;
    public float duration;
    public int damage;
    public DamageType damageType;
    public WeaponType weaponType;
    public float spread = 0.3f;
    public bool isCritical;
}
