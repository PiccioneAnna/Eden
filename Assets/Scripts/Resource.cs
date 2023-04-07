using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : GameObject
{
    public Resource instance;
    public Item properTool;
    public int maxDropCount = 5;
    private int dropCount;

    public Item[] drops;
    public Sprite[] sprites;
    public int health;
    public UnityEngine.GameObject[] droppedObjs;

    Vector3 position;
    Quaternion rotation;

    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;

        System.Random random = new System.Random();

        instance.transform.GetComponent<SpriteRenderer>().sprite = sprites[random.Next(sprites.Length)];

        dropCount = random.Next(5);
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
        System.Random random = new System.Random();
        float offset = (float)random.NextDouble()/2;
        float multplier = offset % 2 == 2 ? 1 : -1;

        if (health <= 0)
        {
            foreach (UnityEngine.GameObject drop in droppedObjs)
            {
                for (int i = 0; i < dropCount; i++)
                {
                    position = new Vector3(position.x, position.y + (multplier * offset), position.z);
                    Instantiate(drop, position, rotation);
                }
            }
            Destroy(instance.gameObject);
        }
    }

}
