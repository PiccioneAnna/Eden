using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : GameObject
{
    public Resource instance;
    public ToolHit toolHit;
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

        toolHit.Hit();
    }
}
