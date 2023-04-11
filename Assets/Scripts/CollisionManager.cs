using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager collisionManager;
    
    public Player player;
    public SceneManager sceneManager;

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
        // Collect dropped materials
        if (collider.gameObject.tag == "Material")
        {
            Debug.Log("Dropped " + collider.gameObject.GetComponentInChildren<Drop>().item);
            if (player.inventoryManager.AddItem(collider.gameObject.GetComponentInChildren<Drop>().item))
            {
                Destroy(collider.gameObject); 
            }
        }
        // Actions that player has to interact to achieve (E)
        else if (player.isInteract)
        {
            // Changing Scene
            if (collider.gameObject.tag == "SceneTrigger")
            {
                ChangeScene(collider.gameObject);
            }

            // using a tool on an item
            if (CorrectTool(collider.gameObject))
            {
                Debug.Log("Damage Taken");
                collider.gameObject.GetComponent<Resource>().TakeDamage();
            }
        }
    }

    void ChangeScene(UnityEngine.GameObject obj)
    {
        string nextScene = obj.GetComponent<SceneTrigger>().nextScene; 
        switch (nextScene)
        {
            case "Pangea":
                sceneManager.LoadPangea();
                break;
            case "Eden":
                sceneManager.LoadEden();
                break;
            case "Purgatory":
                sceneManager.LoadPurgatory();
                break;
        }

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
