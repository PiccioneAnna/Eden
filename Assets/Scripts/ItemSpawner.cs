using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeAgent))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] Item toSpawn;
    [SerializeField] int count;
    [SerializeField] float probability;

    float offsetX;
    float offsetY;
    int multplierX;
    int multplierY;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }

    void Spawn()
    {
        // Randomized drops
        System.Random random = new System.Random();

        if ((float)random.NextDouble() < probability)
        {
            for (int i = 0; i < count; i++)
            {
                // Randomized drop positoning
                random = new System.Random();
                offsetX = (float)random.NextDouble() / 4;
                random = new System.Random();
                offsetY = (float)random.NextDouble() / 8;
                multplierX = offsetX % 2 == 2 ? 1 : -1;
                multplierY = offsetY % 2 == 2 ? 1 : -1;

                // Randomized drop
                Vector3 position = transform.position;
                position = new Vector3(position.x + (multplierX * offsetX), position.y + (multplierY * offsetY), position.z);
                Quaternion rotation = transform.rotation;
                Instantiate(toSpawn, position, rotation);
            }
        }
    }
}
