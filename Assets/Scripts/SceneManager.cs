using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public GameObject[] objs;

    void Awake()
    {
        foreach (var obj in objs)
        {
            DontDestroyOnLoad(obj);
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
