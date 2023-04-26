using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/Tool Action/Return Home")]
public class ReturnHomeAction : ToolAction
{
    public override bool OnApply(Vector2 worldPoint)
    {
        GameSceneManager gameSceneManager = GameManager.instance.gameObject.GetComponent<GameSceneManager>();
        gameSceneManager.InitSwitchScene("Eden", new Vector3(0, 0, 0));
        return true;
    }
}
