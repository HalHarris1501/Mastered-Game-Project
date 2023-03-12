using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct WeaponStruct 
{
    public WeaponType Type;
    public IWeapon weapon;
    public Sprite weaponSprite;
}
