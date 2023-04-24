using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class QuestManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject questPrefab;
    [SerializeField] private Transform questsContent;
    [SerializeField] private UnityEngine.GameObject questHolder;
    public Image QuestUI;

    public List<Quest> CurrentQuests;

    private void Awake()
    {
        questHolder.SetActive(false);
        RefreshQuestList();
    }

    private void RefreshQuestList()
    {
        foreach (var quest in CurrentQuests)
        {
            InitializeQuest(quest);
        }
    }

    private void InitializeQuest(Quest quest)
    {
        quest.Initialize();
        quest.QuestCompleted.AddListener(OnQuestCompleted);

        GameObject questObj = Instantiate(questPrefab, questsContent);
        //questObj.transform.Find("Icon").GetComponent<Image>().sprite = quest.Information.Icon;
        questObj.transform.Find("Title").GetComponent<TMP_Text>().text = quest.Information.Name;

        questObj.GetComponent<Button>().onClick.AddListener(delegate
        {
            questHolder.GetComponent<QuestWindow>().ClearWindow();
            questHolder.GetComponent<QuestWindow>().Initialize(quest);
            questHolder.SetActive(true);
        });
    }

    public void AddQuest(Quest quest)
    {
        CurrentQuests.Add(quest);
    }

    public void Build(string buildingName)
    {
        EventManager.Instance.QueueEvent(new BuildingGameEvent(buildingName));
    }

    public void Hoe()
    {
        EventManager.Instance.TriggerEvent(new HoeGameEvent());
    }

    public void Seed(Crop crop)
    {
        EventManager.Instance.TriggerEvent(new SeedGameEvent(crop));
    }

    public void Harvest(Crop crop)
    {
        EventManager.Instance.TriggerEvent(new HarvestGameEvent(crop));
    }

    public void ClearObject(Resource resource)
    {
        EventManager.Instance.TriggerEvent(new ClearObjectGameEvent(resource));
    }

    public void Visit(string scene)
    {
        EventManager.Instance.TriggerEvent(new VisitGameEvent(scene));
    }

    private void OnQuestCompleted(Quest quest)
    {
        questsContent.GetChild(CurrentQuests.IndexOf(quest)).Find("Checkmark").gameObject.SetActive(true);
    }
}
