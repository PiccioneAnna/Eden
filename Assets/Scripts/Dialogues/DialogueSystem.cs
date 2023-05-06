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

    List<DialogueLine> lines;

    public Button talkBtn;
    public Button shopBtn;
    public Button questsBtn;

    public GameObject intitialOptionsMenu;
    public GameObject talkOptionContainer;
    public GameObject dialogueContainer;

    public Button talkOptionA;
    public Button talkOptionB;
    public Button talkOptionC;

    public TMP_Text talkOptionAText;
    public TMP_Text talkOptionBText;
    public TMP_Text talkOptionCText;

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
                UpdateDialogue();
            }
            TypeOutText();
        }
        else
        {
            ButtonsVisibility(true);
        }
    }

    private void UpdateDialogue()
    {
        // Determine if quest is complete or not to see which dialogue to display
        if (!questManager.CurrentQuests.Contains(currentDialogue.quest) &&
            !questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            lines = currentDialogue.linesBQC;
            PushText();
        }
        else if (questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            lines = currentDialogue.linesAQC;
            PushText();
        }
        else
        {
            Conclude();
        }
    }

    private void PushText()
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
            CycleLine();
        }
    }

    private string DetermineLine(DialogueLine line)
    {
        //If the NPC is talking and there is no playerChoice as well is isnotmultichoice, select first line
        if (line.dialogueType == DialogueType.NPCTalking){
            return line.lineA;
        }
        //If the player is talking then pause the dialogue and show buttons for their choice
        //If the NPC is reponsding then base the line to show off of player choice
        else if(line.dialogueType == DialogueType.NPCResponding)
        {
            switch (line.playerChoice)
            {
                case 0: return line.lineA; break;
                case 1: return line.lineB; break;
                case 2: return line.lineC; break;
            }
        }
        return line.lineA;
    }

    #region Talk Options
    private void ShowTalkOptions(bool b)
    {
        talkOptionContainer.SetActive(b);
        dialogueContainer.SetActive(!b);

        talkOptionA.gameObject.SetActive(b);
        talkOptionB.gameObject.SetActive(b);
        talkOptionC.gameObject.SetActive(b);

        talkOptionAText.text = lines[currentTextLine].lineA;
        talkOptionBText.text = lines[currentTextLine].lineB;
        talkOptionCText.text = lines[currentTextLine].lineC;
    }

    public void SelectionTalkOptionA()
    {
        lineToShow = lines[currentTextLine].lineA;
        lines[currentTextLine + 1].playerChoice = 0;
        currentTextLine += 1;
        UpdateDialogue();
    }

    public void SelectionTalkOptionB()
    {
        lineToShow = lines[currentTextLine].lineB;
        lines[currentTextLine + 1].playerChoice = 1;
        currentTextLine += 1;
        UpdateDialogue();
    }

    public void SelectionTalkOptionC()
    {
        lineToShow = lines[currentTextLine].lineC;
        lines[currentTextLine + 1].playerChoice = 2;
        currentTextLine += 1;
        UpdateDialogue();
    }
    #endregion

    void CycleLine()
    {
        // We don't cycle the line here while the player is selecting a response option
        if (lines[currentTextLine].dialogueType == DialogueType.PlayerTalking)
        {
            ShowTalkOptions(true);
        }
        else
        {
            ShowTalkOptions(false);
            lineToShow = DetermineLine(lines[currentTextLine]);
            totalTimeToType = lineToShow.Length * timePerLetter;
            currentTime = 0f;
            visibleTextPercent = 0f;
            targetText.text = "";
            currentTextLine += 1;
        }
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
        if (gameObject.activeSelf == false)
        {
            Show(true);

            ButtonsVisibility(true);

            dialogueTree = dT;
            player = p;

            UpdatePortrait();
        }
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
            lines = currentDialogue.linesBQC;
            CycleLine();
        }
        else if (questManager.CompletedQuests.Contains(currentDialogue.quest))
        {
            lines = currentDialogue.linesAQC;
            CycleLine();
        }
    }

    private void ButtonsVisibility(bool s)
    {
        intitialOptionsMenu.SetActive(s);

        // Only shows the player the shop if they're the required level
        if(s == true && player.character.level > 1)
        {
            shopBtn.gameObject.SetActive(s);
        }
        else
        {
            shopBtn.gameObject.SetActive(s);
        }

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
            player.character.IncreaseXP(currentDialogue.quest.Reward.xP);
            RecieveItem(currentDialogue.rewardsAQC);
            currentDialogue.dialogueCompletion = true;

            // Remove the required items from the player's inventory
            GiveItem(currentDialogue.removedPlayerItems);

            // Makes the target quest crop available within the shop
            foreach (Item item in currentDialogue.addedShopItems)
            {
                item.shopItem = true;
            }
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
                }
                Debug.Log("Recieved " + rewards[i].itemCount + " " + rewards[i].item);
            }
        }
    }

    // Removes amount of items to inventory if there are any to be added
    private void GiveItem(List<Reward> rewards)
    {
        if (rewards.Count != 0)
        {
            for (int i = 0; i < rewards.Count; i++)
            {
                for (int j = 0; j < rewards[i].itemCount; j++)
                {
                    player.inventoryManager.RemoveItem(rewards[i].item);
                }
                Debug.Log("Gave away " + rewards[i].itemCount + " " + rewards[i].item);
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
