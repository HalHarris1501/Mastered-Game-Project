using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct Attribute
{
    public AttributeType attributeType;
    public int attributeScore;
    public int AttributeModifier()
    {
        int modifier = Mathf.FloorToInt((attributeScore - 10) / 2);
        return modifier;
    }
}
