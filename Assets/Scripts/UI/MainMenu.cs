using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string nameEssentialScene;
    [SerializeField] string nameNewGameStartScene;

    //Data Containers that are cleared or loaded based on game data
    [SerializeField] DialogueTree[] dialogueTrees;
    [SerializeField] CropsContainer[] crops;
    [SerializeField] PlaceableObjectsContainer[] placedObjects;

    public void ExitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void StartNewGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);

        // Takes all the games data containers and clears them && refreshes quest list back to the beginning 
        foreach (DialogueTree tree in dialogueTrees)
        {
            tree.Refresh();
        }
        foreach (CropsContainer crop in crops)
        {
            crop.Clear();
        }
        foreach (PlaceableObjectsContainer obj in placedObjects)
        {
            obj.Clear();
        }
    }

    public void LoadGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameNewGameStartScene, LoadSceneMode.Single);
        UnityEngine.SceneManagement.SceneManager.LoadScene(nameEssentialScene, LoadSceneMode.Additive);
    }
}
