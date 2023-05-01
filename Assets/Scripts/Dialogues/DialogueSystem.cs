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

    public DialogueTree dialogueTree;
    DialogueContainer currentDialogue;
    int currentTextLine;

    public Player player;
    public QuestManager questManager;
    public ShopManager shopManager;

    public Button talkBtn;
    public Button shopBtn;
    public Button questsBtn;

    [Range(0f,1f)]
    [SerializeField] float visibleTextPercent;
    [SerializeField] float timePerLetter = 0.05f;
    float totalTimeToType, currentTime;
    string lineToShow;

    private void Update()
    {
        if(currentDialogue != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Determine if quest is complete or not to see which dialogue to display
                if (!questManager.CurrentQuests.Contains(currentDialogue.quest) &&
                    !questManager.CompletedQuests.Contains(currentDialogue.quest))
                {
                    PushText(currentDialogue.linesBQC);
                }
                else if(questManager.CompletedQuests.Contains(currentDialogue.quest))
                {
                    PushText(currentDialogue.linesAQC);
                }
                else
                {
                    Conclude();
                }
            }
            TypeOutText();
        }
    }

    private void PushText(List<string> lines)
    {
        if(visibleTextPercent < 1f)
        {
            visibleTextPercent = 1f;
            UpdateText();
            return;
        }

        if(currentTextLine >= lines.Count)
        {
            Conclude();
        }
        else
        {
            CycleLine(lines);
        }
    }

    void CycleLine(List<string> lines)
    {
        lineToShow = lines[currentTextLine];
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

    public void Initialize(DialogueTree dT, Player p)
    {
        Show(true);

        ButtonsVisibility(true);

        dialogueTree = dT;
        player = p;

        UpdatePortrait();
    }

    public void Talk()
    {
        Debug.Log("Talked to character");
    }

    public void Shop()
    {
        Debug.Log("Opened Shop");
        shopManager.Refresh();
    }

    public void Quests()
    {
        Debug.Log("Opened Quests");
        ButtonsVisibility(false);

        currentDialogue = dialogueTree.GetCurrentDialogue();

        if(currentDialogue == null) { return; }

        currentTextLine = 0;
        player.isInteract = true;

        // Determine if quest is complete or not to see which dialogue to display
        if (!questManager.CurrentQuests.Contains(currentDialogue.quest) &&
            !questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            CycleLine(currentDialogue.linesBQC);
        }
        else if (questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            CycleLine(currentDialogue.linesAQC);
        }
    }

    private void ButtonsVisibility(bool s)
    {
        shopBtn.gameObject.SetActive(s);
        talkBtn.gameObject.SetActive(s);
        questsBtn.gameObject.SetActive(s);
        targetText.gameObject.SetActive(!s);
    }

    private void UpdatePortrait()
    {
        portrait.sprite = dialogueTree.actor.portrait;
        nameText.text = dialogueTree.actor.Name;
    }

    private void Show(bool s)
    {
        gameObject.SetActive(s);
    }

    private void Conclude()
    {
        Debug.Log("The dialogue has ended");
        player.isInteract = false;
        if(!questManager.CurrentQuests.Contains(currentDialogue.quest) &&
           !questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            RecieveItem(currentDialogue.rewardsBQC);
            RecieveQuest();
        }
        else if(questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            RecieveItem(currentDialogue.rewardsAQC);
        }

        currentDialogue = null;
        Show(false);
    }

    #region rewards

    // Adds amount of items to inventory if there are any to be added
    private void RecieveItem(List<Reward> rewards)
    {
        if (rewards.Count != 0)
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                for (int j = 0; j < rewards[i].itemCount; j++)
                {
                    player.inventoryManager.AddItem(rewards[i].item);
                    Debug.Log("Recieved " + rewards[i].itemCount + " " + rewards[i].item);
                }
            }
        }
    }

    // Adds a quest to quest manager
    private void RecieveQuest()
    {
        if(currentDialogue.quest != null && 
            !questManager.CurrentQuests.Contains(currentDialogue.quest) &&
            !questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            questManager.AddQuest(currentDialogue.quest);
            Debug.Log("Quest Active : " + currentDialogue.quest.name);
        }
    }

    #endregion
}
