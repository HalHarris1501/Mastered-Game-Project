using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemValues", menuName = "ShopItem")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemSprite;
    public int itemCost;
}
