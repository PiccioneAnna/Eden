using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : Object
{
    public Item item;
    private Player player;
    public float distance;

    [SerializeField] float speed = 2f;
    [SerializeField] float pickUpDistance = 1.5f;
    [SerializeField] float ttl = 10f;

    private void Awake()
    {
        player = GameObject.Find("GameManager").GetComponent<GameManager>().player;
    }

    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > pickUpDistance)
        {
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if(distance < .1f)
        {
            if (player.inventoryManager.AddItem(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
