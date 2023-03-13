using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponValues", menuName = "Weapons/WeaponData")]
public class WeaponDataPack : ScriptableObject
{
    public RuntimeAnimatorController animation;
    public WeaponType weaponType;
    public List<DamageStruct> baseDamage;
    public string[] properties = new string[0];
    public BoxCollider2D weaponCollider;
    public Sprite weaponSprite;
}
