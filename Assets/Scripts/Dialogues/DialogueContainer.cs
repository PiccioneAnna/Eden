using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Reward
{
    public Item item;
    public int itemCount;
}

/// <summary>
/// Dialogue container consists of NPC Dialogue, rewards, and quest to activate
/// </summary>
[CreateAssetMenu(menuName = "Data/Dialogue/Container")]
public class DialogueContainer : ScriptableObject
{
    public List<string> linesBQC; // Before Quest Completion
    public List<string> linesAQC; // After Quest Completion
    public List<Reward> rewardsBQC; // Before Quest Completion Rewards
    public List<Reward> rewardsAQC; // After Quest Completion Rewards
    public Quest quest;
    public int levelRequirement;
    public bool questCompletion;
}
