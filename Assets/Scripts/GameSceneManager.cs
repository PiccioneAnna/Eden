using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager instance;
    public UnityEngine.GameObject player;

    private void Awake()
    {
        instance = this;
    }

    string currentScene;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
    }

    public void SwitchScene(string to, Vector3 targetPosition)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(to, LoadSceneMode.Additive);
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currentScene);
        currentScene = to;
        Transform playerTransform = player.transform;
        playerTransform.position = new Vector3(
            targetPosition.x, 
            targetPosition.y, 
            playerTransform.position.z);
    }
}
