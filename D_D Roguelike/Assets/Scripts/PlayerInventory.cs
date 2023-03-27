using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private GameObject itemUITemplate;
    [SerializeField] private RectTransform itemContainer;
    [SerializeField] private List<InventoryItem> inventoryItems;

    private Dictionary<ItemType, GameObject> _inventoryItemUIs = new Dictionary<ItemType, GameObject>();

    //Singleton pattern
    #region Singleton
    private static PlayerInventory _instance;
    public static PlayerInventory Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlayerInventory>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("PlayerInventory");
                _instance = go.AddComponent<PlayerInventory>();
            }
            return _instance;
        }
    }
    #endregion

    private void Awake()
    {
        if (_instance == null) //if there's no instance of the weapon manager, make this the weapons manager, ortherwise delete this to avoid duplicates
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        itemUITemplate.gameObject.SetActive(false);
        GenerateItems();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateItems()
    {
        foreach (InventoryItem item in inventoryItems)
        {
            MakeItem(item);
        }
    }

    private void MakeItem(InventoryItem item)
    {
        GameObject newItem = Instantiate(itemUITemplate, itemContainer);
        newItem.name = item.itemData.itemType.ToString();
        newItem.GetComponent<ShopItemUI>().SetItemInfo(item.itemData.itemSprite, item.itemData.itemType, item.itemData.itemCost, item.stock);
        newItem.GetComponent<ShopItemUI>().itemButton.onClick.AddListener(() => SellItem(item.itemData.itemType));
        newItem.SetActive(true);
        _inventoryItemUIs.Add(item.itemData.itemType, newItem);
    }

    void SellItem(ItemType itemType)
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            if (itemType == inventoryItem.itemData.itemType)
            {
                MoneyManager.Instance.AddMoney(inventoryItem.itemData.itemCost);
                DecreaseStock(inventoryItem);
                ShopManager.Instance.AddShopItem(inventoryItem);
            }
        }
    }

    private void DecreaseStock(InventoryItem inventoryItem)
    {
        inventoryItem.stock--;
        _inventoryItemUIs[inventoryItem.itemData.itemType].GetComponent<ShopItemUI>().SetItemStock(inventoryItem.stock);
        if (inventoryItem.stock <= 0)
        {
            RemoveItem(inventoryItem.itemData.itemType);
        }
    }

    public void RemoveItem(ItemType type)
    {
        if (_inventoryItemUIs.ContainsKey(type))
        {
            GameObject tempObject = _inventoryItemUIs[type];
            _inventoryItemUIs.Remove(type);
            Destroy(tempObject);
        }
    }

    public void AddItem(InventoryItem item)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if(inventoryItems[i].itemData.itemType == item.itemData.itemType)
            {
                IncreaseStock(inventoryItems[i]);
                if (_inventoryItemUIs.ContainsKey(inventoryItems[i].itemData.itemType))
                {
                    _inventoryItemUIs[inventoryItems[i].itemData.itemType].GetComponent<ShopItemUI>().SetItemStock(inventoryItems[i].stock);
                }
                else
                {
                    MakeItem(inventoryItems[i]);
                }
                return;
            }
        }

        InventoryItem newItem = new InventoryItem();
        newItem.stock = 1;
        newItem.itemData = item.itemData;
        inventoryItems.Add(newItem);
        MakeItem(newItem);
    }

    private void IncreaseStock(InventoryItem item)
    {
        item.stock++;
    }
}
