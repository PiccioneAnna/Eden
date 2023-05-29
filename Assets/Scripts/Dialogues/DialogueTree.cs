using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue/Tree")]
public class DialogueTree : ScriptableObject
{
    public List<DialogueContainer> dialogueContainers;

    public DialogueContainer taskContainer;

    public Actor actor;

    private int currentPlayerLevel;

    private int dialogueIndex = 0;

    private void Refresh()
    {
        // Temporary quest reset for now, long term would be refresh upon new save file
        foreach (DialogueContainer dialogue in dialogueContainers)
        {
            dialogue.dialogueCompletion = false;
        }
    }
    
    // Theoretically goes through dialogue list and returns dialogue matching player's level
    public DialogueContainer GetCurrentDialogue()
    {
        currentPlayerLevel = GameManager.instance.player.GetComponent<Character>().level;

        DialogueContainer dialogue = dialogueContainers[dialogueIndex];

        if (dialogue.dialogueCompletion == true)
        {
            dialogueIndex++;
        }

        if (dialogue.levelRequirement <= currentPlayerLevel && dialogue.dialogueCompletion == false)
        {
            if(dialogue.quests.Count > 0)
            {
                dialogue.quests[0].Initialize();
            }
            return dialogue;
        }

        return null;
    }

    // return a generic task dialogue if player chooses task


    // returns a generic talk dialogue if player chooses talk
}
