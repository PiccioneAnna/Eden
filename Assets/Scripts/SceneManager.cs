using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public static SceneManager sceneManager;

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (sceneManager == null)
        {
            sceneManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadEden()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Eden"); 
    }

    public void LoadPangea()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Pangea");
    }

    public void LoadPurgatory()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Purgatory");
    }
}
