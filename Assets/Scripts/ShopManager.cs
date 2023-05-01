using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ShopManager : MonoBehaviour
{
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portrait;

    public InventoryManager inventoryManager;
    public Player player;

    int playerLevel;

    public List<Item> merchantItems;
    public List<Item> availableMerchantItems;

    public List<GameObject> merchantItemsUI;
    public List<GameObject> playerItemsUI;

    public GameObject merchantInventory;
    public GameObject playerInventory;
    public GameObject shopitemPrefab;

    public void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        // Gets Merchant Items
        foreach (Item item in merchantItems)
        {
            GameObject shopItem = GameObject.Instantiate(shopitemPrefab, merchantInventory.transform);
            shopItem.GetComponent<ShopItem>().item = item;
            shopItem.GetComponent<ShopItem>().Refresh();
            merchantItemsUI.Add(shopItem);
        }

        // Gets Available Player Items
        foreach (InventorySlot slot in inventoryManager.inventorySlots)
        {
            if (slot.item != null && slot.item.itemType == Item.ItemType.Crop)
            {
                CreateShopItemInPlayerInventory(slot.item, slot.gameObject.GetComponentInChildren<InventoryItem>().count);
            }
        }
    }

    public void CreateShopItemInPlayerInventory(Item item, int count = 1)
    {

        // If the item is not in the player's inventory then create a new UI slot for it, otherwise add to count
        foreach (GameObject go in playerItemsUI)
        {
            if(go.GetComponent<ShopItem>().item == item)
            {
                go.GetComponent<ShopItem>().itemCount += 1;
                go.GetComponent<ShopItem>().Refresh();
                return;
            }
        }

        GameObject shopItem = GameObject.Instantiate(shopitemPrefab, playerInventory.transform);
        shopItem.GetComponent<ShopItem>().item = item;
        shopItem.GetComponent<ShopItem>().itemCount += count;
        shopItem.GetComponent<ShopItem>().Refresh();
        shopItem.GetComponent<ShopItem>().inPlayerInventory = true;
        playerItemsUI.Add(shopItem);
    }
}
