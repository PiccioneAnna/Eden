using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : Object
{
    public Resource instance;
    public ToolHit toolHit;
    public Item properTool;
    public string name;
    public int maxDropCount = 5;
    public int minDropCount = 1;
    public int maxScale = 9;
    public int minScale = 2;
    public bool multisprite = false;
    private int dropCount;

    public Item[] drops;
    public UnityEngine.GameObject[] droppedObjs;
    public Sprite[] sprites;
    public int health;
    private int originalHealth;

    Quaternion rotation;

    void Awake()
    {
        position = instance.transform.position;
        rotation = instance.transform.rotation;

        originalHealth = health;

        System.Random random = new System.Random();

        if (instance.transform.GetComponent<SpriteRenderer>() != null && multisprite)
        {
            instance.transform.GetComponent<SpriteRenderer>().sprite = sprites[random.Next(sprites.Length)];
        }
    }

    public void Shake()
    {
        anim.SetTrigger("Shake");
    }
    
    // Changes a tree's display based on health
    public void HitTree(int h)
    {
        float healthPercent = (float)h / (float)originalHealth;
        Debug.Log("Current Health: " + h);
        Debug.Log("Old Health: " + originalHealth);
        Debug.Log("health percent" + healthPercent);
        int sprite = 0;

        if (healthPercent >= .9f)
        {
            return;
        }
        else if (healthPercent >= .7f)
        {
            sprite = 1;
        }
        else if (healthPercent >= .5f)
        {
            sprite = 2;
        }
        else
        {
            sprite = 3;
        }

        if (instance.transform.GetComponent<SpriteRenderer>() != null && sprites != null)
        {
            instance.transform.GetComponent<SpriteRenderer>().sprite = sprites[sprite];
        }
    }

    public void TakeDamage()
    {
        health--;
        Debug.Log(health);

        toolHit.Hit();
    }
}
