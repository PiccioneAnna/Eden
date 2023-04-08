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
            // Changing Scene
            if (collision.gameObject.tag == "SceneTrigger")
            {
                ChangeScene(collision.gameObject);
            }

            // using a tool on an item
            if (CorrectTool(collision.gameObject))
            {
                Debug.Log("Damage Taken");
                collision.gameObject.GetComponent<Resource>().TakeDamage();
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
