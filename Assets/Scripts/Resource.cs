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
    public float maxScale = 3;
    public float minScale = 2;
    public bool multisprite = false;
    private int dropCount;

    public Item[] drops;
    public Sprite[] sprites;
    public Sprite selectedSprite;
    public Vector3[] rotations;
    public Vector3[] positions;
    public int health;
    private int originalHealth;

    void Awake()
    {
        instance = this;

        originalHealth = health;

        DetermineSprite();
    }

    public void DetermineSprite()
    {
        System.Random random = new System.Random();

        if (instance.transform.GetComponent<SpriteRenderer>() != null && multisprite && selectedSprite == null)
        {
            instance.transform.GetComponent<SpriteRenderer>().sprite = sprites[random.Next(sprites.Length)];
            selectedSprite = instance.transform.GetComponent<SpriteRenderer>().sprite;
        }
        else if (selectedSprite != null && multisprite)
        {
            instance.transform.GetComponent<SpriteRenderer>().sprite = selectedSprite;
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
        int index = 0;

        if (healthPercent >= .9f)
        {
            return;
        }
        else if (healthPercent >= .5f)
        {
            index = 1;
        }
        else
        {
            index = 2;
        }

        //Vector3 newRotation = rotations[index];
        //Vector3 newPosition = positions[index];

        //gameObject.transform.rotation = Quaternion.Euler(newRotation);
        //gameObject.transform.localPosition = newPosition;
    }

    public void TakeDamage() 
    {
   
        health--;
   
        Debug.Log(health);
   
        toolHit.Hit();  
    }
}
