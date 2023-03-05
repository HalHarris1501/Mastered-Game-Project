using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CreatureAttributes", menuName = "CreatureAttributes")]
[System.Serializable]
public class CreatureAttributes : ScriptableObject
{
    public Attribute Strength;
    public Attribute Dexterity;
    public Attribute Constitution;
    public Attribute Intelligence;
    public Attribute Wisdom;
    public Attribute Charisma;
}
