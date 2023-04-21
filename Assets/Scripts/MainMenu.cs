using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nameEssentialScene;
    [SerializeField] string nameNewGameStartScene;

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }
}
