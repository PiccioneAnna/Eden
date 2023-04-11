using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : GameObject
{
    public Resource instance;
    public Item properTool;
    public int maxDropCount = 5;
    public int minDropCount = 1;
    private int dropCount;

    public Item[] drops;
    public UnityEngine.GameObject[] droppedObjs;
    public Sprite[] sprites;
    public int health;

    Quaternion rotation;

    void Awake()
    {
        position = instance.transform.position;
        rotation = instance.transform.rotation;

        System.Random random = new System.Random();

        if (instance.transform.GetComponent<SpriteRenderer>() != null)
        {
            instance.transform.GetComponent<SpriteRenderer>().sprite = sprites[random.Next(sprites.Length)];

        }
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }

    public void TakeDamage()
    {
        Shake();
        health--;
        Debug.Log(health);

        // Randomized drops
        UnityEngine.GameObject drop;
        System.Random random = new System.Random();
        float offsetX;
        float offsetY;
        int multplierX;
        int multplierY;

        dropCount = random.Next(maxDropCount) + minDropCount;

        if (health <= 0)
        {
            Debug.Log("Drop Count:" + dropCount);
            for (int i = 0; i < dropCount; i++)
            {
                random = new System.Random();
                drop = droppedObjs[random.Next(droppedObjs.Length)];

                // Randomized drop positoning
                random = new System.Random();
                offsetX = (float)random.NextDouble() / 4;
                random = new System.Random();
                offsetY = (float)random.NextDouble() / 8;
                multplierX = offsetX % 2 == 2 ? 1 : -1;
                multplierY = offsetY % 2 == 2 ? 1 : -1;

                // Randomized drop
                position = new Vector3(position.x + (multplierX * offsetX), position.y + (multplierY * offsetY), position.z);
                Instantiate(drop, position, rotation);
            }
            Destroy(instance.gameObject);
        }
    }

}
