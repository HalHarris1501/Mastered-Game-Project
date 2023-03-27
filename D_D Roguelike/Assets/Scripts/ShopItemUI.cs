using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    //item type elements
    private ItemType itemType;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;

    //item cost elements
    [SerializeField] private TMP_Text itemCostText;
    private float itemCost = 0.00f;

    //item stock elements
    [SerializeField] private TMP_Text itemStockText;
    private int itemStock;

    //button element
    public Button itemButton;
    bool costFine = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo(Sprite itemPic, ItemType itemName, int itemCost, int itemStock)
    {
        SetItemImage(itemPic);
        SetItemName(itemName);
        SetItemCost(itemCost);
        SetItemStock(itemStock);
    }

    public void SetItemImage(Sprite itemPic)
    {
        itemImage.sprite = itemPic;
    }

    public void SetItemName(ItemType itemName)
    {
        itemType = itemName;
        itemNameText.text = itemName.ToString();
    }

    public void SetItemCost(float itemCost)
    {
        this.itemCost = itemCost;
        itemCostText.text = "Price: " + this.itemCost + "G";
    }

    public void SetItemCost(float itemCost, float money)
    {
        this.itemCost = itemCost;
        itemCostText.text = "Price: " + this.itemCost + "G";
        CheckCost(money);
    }

    public void SetItemStock(int itemStock)
    {
        this.itemStock = itemStock;
        itemStockText.text = "Stock: " + this.itemStock;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public void CheckCost(float money)
    {
        if(money < itemCost)
        {
            itemCostText.color = Color.red;
            costFine = false;
        }
        else 
        {
            itemCostText.color = Color.black;
            costFine = true;
        }
        ToggleButton();
    }

    public void ToggleButton()
    {
        if (costFine)
        {
            itemButton.interactable = true;
        }
        else
        {
            itemButton.interactable = false;
        }
    }
}
