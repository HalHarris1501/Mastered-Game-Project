using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "CreatureAttributes", menuName = "CreatureAttributes")]
[System.Serializable]
public class CreatureAttributes : ScriptableObject
{
    [SerializeField] private Attribute Strength = new Attribute(AttributeType.Strength);
    [SerializeField] private Attribute Dexterity = new Attribute(AttributeType.Dexterity);
    [SerializeField] private Attribute Constitution = new Attribute(AttributeType.Constitution);
    [SerializeField] private Attribute Intelligence = new Attribute(AttributeType.Intelligence);
    [SerializeField] private Attribute Wisdom = new Attribute(AttributeType.Wisdom);
    [SerializeField] private Attribute Charisma = new Attribute(AttributeType.Charisma);    
}
