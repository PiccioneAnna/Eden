using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] TMP_Text targetText;
    [SerializeField] TMP_Text nameText;
    [SerializeField] Image portrait;

    DialogueContainer currentDialogue;
    int currentTextLine;

    public Player player;
    public QuestManager questManager;

    [Range(0f,1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    string lineToShow;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PushText();
        }
        TypeOutText();
    }

    private void PushText()
    {
        if(visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if(currentTextLine >= currentDialogue.line.Count)
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
        lineToShow = currentDialogue.line[currentTextLine];
        totalTimeToType = lineToShow.Length * timePerLetter;
        currentTime = 0f;
        visibleTextPercent = 0f;
        targetText.text = "";

        currentTextLine += 1;
    }

    private void TypeOutText()
    {
        if(visibleTextPercent >= 1f) { return; }

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

    public void Initialize(DialogueContainer dialogueContainer, Player p)
    {
        Show(true);
        currentDialogue = dialogueContainer;
        currentTextLine = 0;
        player = p;
        player.isInteract = true;
        CycleLine();
        UpdatePortrait();
    }

    private void UpdatePortrait()
    {
        portrait.sprite = currentDialogue.actor.portrait;
        nameText.text = currentDialogue.actor.Name;
    }

    private void Show(bool s)
    {
        gameObject.SetActive(s);
    }

    private void Conclude()
    {
        Debug.Log("The dialogue has ended");
        player.isInteract = false;
        RecieveItem();
        RecieveQuest();
        Show(false);
    }

    // Adds amount of items to inventory if there are any to be added
    private void RecieveItem()
    {
        if (currentDialogue.items.Count != 0)
        {
            for (int i = 0; i < currentDialogue.items.Count; i++)
            {
                for (int j = 0; j < currentDialogue.itemsCount.Count; j++)
                {
                    player.inventoryManager.AddItem(currentDialogue.items[i]);
                    Debug.Log("Recieved " + currentDialogue.itemsCount[i] + " " + currentDialogue.items[i]);
                }
            }
        }
    }

    // Adds a quest to quest manager
    private void RecieveQuest()
    {
        if(currentDialogue.quests.Count != 0)
        {
            for (int i = 0; i < currentDialogue.quests.Count; i++)
            {
                questManager.AddQuest(currentDialogue.quests[i]);
                Debug.Log("Quest Active : " + currentDialogue.quests[i].name);
            }
        }
    }
}
