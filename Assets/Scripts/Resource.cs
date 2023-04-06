using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : GameObject
{
    public Resource instance;
    public Item properTool;

    public Item[] drops;
    public int health;
    public UnityEngine.GameObject[] droppedObjs;

    Vector3 position;
    Quaternion rotation;
    
    void Awake()
    {
        position = transform.position;
        rotation = transform.rotation;

        DontDestroyOnLoad(this.gameObject);
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

        if(health <= 0)
        {
            foreach (UnityEngine.GameObject drop in droppedObjs)
            {
                Instantiate(drop, position, rotation);
            }
            Destroy(instance.gameObject);
        }
    }

}
