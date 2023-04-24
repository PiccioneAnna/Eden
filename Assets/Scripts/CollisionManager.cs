using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager collisionManager;
    
    public Player player;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (collisionManager == null)
        {
            collisionManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        DetermineCollisionType(collider);
    }

    void DetermineCollisionType(Collider2D collider)
    {

    }

    public bool CorrectTool(UnityEngine.GameObject obj)
    {
        // Checks for tool & resource iteraction
        if (player != null && player.selectedItem != null &&
            player.selectedItem.itemName == obj.GetComponentInChildren<Resource>().properTool.itemName)
        {
            return true;
        }
        return false;
    }
}
