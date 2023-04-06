using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public Player player;

    void OnCollisionStay2D(Collision2D collision)
    {
        DetermineCollisionType(collision);
    }

    void DetermineCollisionType(Collision2D collision)
    {
        // Collect dropped materials
        if (collision.gameObject.tag == "Material")
        {
            Debug.Log("Dropped" + collision.gameObject.GetComponentInChildren<Drop>().item);
            if (player.inventoryManager.AddItem(collision.gameObject.GetComponentInChildren<Drop>().item))
            {
                Destroy(collision.gameObject); 
            }
        }
        // Actions that player has to interact to achieve (E)
        else if (player.isInteract)
        {
            if (CorrectTool(collision.gameObject))
            {
                Debug.Log("Damage Taken");
                collision.gameObject.GetComponent<Resource>().TakeDamage();
            }
        }
    }

    public bool CorrectTool(UnityEngine.GameObject obj)
    {
        // Checks for tool & resource iteraction
        if (player.selectedItem != null && 
            player.selectedItem.itemName == obj.GetComponentInChildren<Resource>().properTool.itemName)
        {
            return true;
        }
        return false;
    }
}
