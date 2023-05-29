using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public struct CSDData
{
    public CutSceneDialogueContainer dialogue;
    public string nextScene;
    public Vector3 position;
}

public class CutSceneDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text targetText;

    public List<CSDData> dialogues;

    public static int dialogueIndex;

    public CutSceneDialogueContainer currentDialogue;

    public GameObject dialogueContainer;
    public string NextScene;
    int currentTextLine;

    [Range(0f, 1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;

    string lineToShow;

    public void Awake()
    {
        currentDialogue = dialogues[dialogueIndex].dialogue;
        if (gameObject.activeSelf == false)
        {
            Show(true);
        }
    }

    private void Update()
    {
        if (currentDialogue != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PushText();
            }
            TypeOutText();
        }
    }

    private void PushText()
    {
        if (visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if (currentTextLine >= currentDialogue.lines.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine();
        }
    }

    void CycleLine()
    {
        lineToShow = currentDialogue.lines[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";
        currentTextLine += 1;
    }

    private void TypeOutText()
    {
        if (visibleTextPercent >= 1f) { return; }

        currentTime += Time.deltaTime;
        visibleTextPercent = currentTime / totalTimeToType;
        visibleTextPercent = Mathf.Clamp(visibleTextPercent, 0, 1f);
        UpdateText();
    }

    void UpdateText()
    {
        int letterCount = (int)(lineToShow.Length * visibleTextPercent);
        targetText.text = lineToShow.Substring(0, letterCount);
    }

    private void Show(bool s)
    {
        gameObject.SetActive(s);
    }

    public void Conclude()
    {
        currentDialogue = null;
        Show(false);

        if(dialogueIndex == 0)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(dialogues[dialogueIndex].nextScene, LoadSceneMode.Single);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Essential", LoadSceneMode.Additive);
        }
        else
        {
            GameSceneManager.instance.InitSwitchScene(dialogues[dialogueIndex].nextScene, dialogues[dialogueIndex].position);
        }

        dialogueIndex++;
    }

}
