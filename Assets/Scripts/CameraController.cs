using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController cameraController;
    //Public variable to store a reference to the player game object
    public Player player;        
    //Private variable to store the offset distance between the player and camera
    private Vector3 offset;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (cameraController == null)
        {
            cameraController = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
    }

    // LateUpdate is called after Update each frame
    void LateUpdate()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + offset;
    }
}
