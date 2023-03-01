using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileValues", menuName = "Weapons/ProjectileData")]
public class ProjectileDataPack : ScriptableObject
{
    public Sprite sprite;
    public Transform transform;
    public bool isAmmo;
    public float moveSpeed;
    public float duration;
    public DamageType damageType;
    public WeaponType weaponType;
    public float spread = 0.3f;
}
