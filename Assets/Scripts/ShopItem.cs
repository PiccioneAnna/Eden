using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public class ShopItem : MonoBehaviour
{
    public ShopItem shopItem;

    public Item item;
    public int itemCount;
    public int sellPrice;
    public int buyPrice;
    public bool inPlayerInventory = false;

    public InventoryManager inventoryManager;
    public ShopManager shopManager;

    public Image itemImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public TMP_Text countText;

    private void Awake()
    {
        shopItem = this;
        inventoryManager = GameManager.instance.GetComponent<InventoryManager>();
        shopManager = gameObject.transform.parent.parent.GetComponent<ShopManager>();
    }

    public void Refresh()
    {
        itemImage.sprite = item.image;
        priceText.text = sellPrice.ToString();
        countText.text = "x" + itemCount.ToString();
        nameText.text = item.name;

        // If there is only one object then don't display the count text
        if(itemCount == 1)
        {
            countText.text = "";
        }
    }

    public void SellObject()
    {
        // If the item is player's remove from inventory and get money
        if (inPlayerInventory)
        {
            if(itemCount == 1)
            {
                Destroy(gameObject);
                shopManager.playerItemsUI.Remove(gameObject);
                return;
            }
            else
            {
                itemCount--;
                Refresh();
                return;
            }

            inventoryManager.RemoveItem(item);
        }
        // If the item is the merchant's add to inventory and decrease money
        else
        {
            inventoryManager.AddItem(item);
            shopManager.CreateShopItemInPlayerInventory(item);
        }
    }
}
