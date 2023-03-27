using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour, IObserver<float>
{
    [SerializeField] private GameObject shopItemUITemplate;
    [SerializeField] private RectTransform itemContainer;
    [SerializeField] private List<InventoryItem> shopItems;

    private Dictionary<ItemType, GameObject> _shopItemUIs = new Dictionary<ItemType, GameObject>();

    //Singleton pattern
    #region Singleton
    private static ShopManager _instance;
    public static ShopManager Instance
    {
        get //making sure that a weapon manager always exists
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ShopManager>();
            }
            if (_instance == null)
            {
                GameObject go = new GameObject("ShopManager");
                _instance = go.AddComponent<ShopManager>();
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
        MoneyManager.Instance.RegisterObserver(this);
        shopItemUITemplate.gameObject.SetActive(false);
        GenerateItems();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GenerateItems()
    {
        foreach(InventoryItem item in shopItems)
        {
            MakeShopItem(item);
        }
    }

    private void MakeShopItem(InventoryItem item)
    {
        GameObject newItem = Instantiate(shopItemUITemplate, itemContainer);
        newItem.name = item.itemData.itemType.ToString();
        newItem.GetComponent<ShopItemUI>().SetItemInfo(item.itemData.itemSprite, item.itemData.itemType, item.itemData.itemCost, item.stock);
        newItem.GetComponent<ShopItemUI>().itemButton.onClick.AddListener(() => PurchaseItem(item.itemData.itemType));
        newItem.SetActive(true);
        _shopItemUIs.Add(item.itemData.itemType, newItem);
    }

    public void NewItemAdded(float money)
    {

    }

    public void ItemRemoved(float money)
    {

    }

    public void ItemAltered(float money, int count)
    {
        foreach(InventoryItem shopItem in shopItems)
        {
            AlterPurchaseable(money, shopItem.itemData.itemType);
        }
    }

    public void AlterPurchaseable(float money, ItemType itemType)
    {
        if(_shopItemUIs.ContainsKey(itemType))
        {
            _shopItemUIs[itemType].GetComponent<ShopItemUI>().CheckCost(money);
        }
    }

    private void RemoveShopItem(ItemType type)
    {
        if(_shopItemUIs.ContainsKey(type))
        {
            GameObject tempObject = _shopItemUIs[type];
            _shopItemUIs.Remove(type);
            Destroy(tempObject);
        }
    }

    public void AddShopItem(InventoryItem item)
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            if (shopItems[i].itemData.itemType == item.itemData.itemType)
            {
                IncreaseStock(shopItems[i]);
                if (_shopItemUIs.ContainsKey(shopItems[i].itemData.itemType))
                {
                    _shopItemUIs[shopItems[i].itemData.itemType].GetComponent<ShopItemUI>().SetItemStock(shopItems[i].stock);
                }
                else
                {
                    MakeShopItem(shopItems[i]);
                }
                return;
            }
        }

        InventoryItem newItem = new InventoryItem();
        newItem.stock = 1;
        newItem.itemData = item.itemData;
        shopItems.Add(newItem);
        MakeShopItem(newItem);
    }

    void PurchaseItem(ItemType itemType)
    {
        foreach (InventoryItem shopItem in shopItems)
        {
            if (itemType == shopItem.itemData.itemType)
            {
                MoneyManager.Instance.AddMoney(-(shopItem.itemData.itemCost));
                DecreaseStock(shopItem);
                PlayerInventory.Instance.AddItem(shopItem);
            }
        }
    }

    private void DecreaseStock(InventoryItem shopItem)
    {
        shopItem.stock--;
        _shopItemUIs[shopItem.itemData.itemType].GetComponent<ShopItemUI>().SetItemStock(shopItem.stock);
        if(shopItem.stock <= 0)
        {
            RemoveShopItem(shopItem.itemData.itemType);
        }
    }

    private void IncreaseStock(InventoryItem shopItem)
    {
        shopItem.stock++;
    }

    public void ItemCountAltered(float type, int count)
    {
        throw new System.NotImplementedException();
    }
}
