using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponValues", menuName = "Weapons/WeaponData")]
public class WeaponDataPack : ScriptableObject
{
    public Animation animation;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private List<DamageStruct> damage;
    [SerializeField] private string[] properties = new string[0];
}
